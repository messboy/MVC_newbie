using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_Backend.Helpers;
using MVC_Backend.Models;
using MVC_Backend.ViewModels;
using NLog;
using PagedList;
using System.Text.RegularExpressions;
using System.IO;
using System.Web.Helpers;


namespace MVC_Backend.Controllers
{
	public class ArticlesController : Controller
	{
		private WorkshopEntities db = new WorkshopEntities();
		private static Logger logger = LogManager.GetCurrentClassLogger();
		// GET: Articles
		public ActionResult Index(QueryOption<Article> queryOption)
		{
			//logger.Log(LogLevel.Info, "Sample informational message");
            
            // Articles 關聯 Category table
			var query = db.Articles.Include(a => a.Category);

            // 查詢資料
			if (!string.IsNullOrEmpty(queryOption.Keyword))
			{
				query = query.Where(a => a.Subject.Contains(queryOption.Keyword)
											   ||
											   a.Summary.Contains(queryOption.Keyword)
											   ||
											   a.ContentText.Contains(queryOption.Keyword)
											   ||
											   a.Category.Name.Contains(queryOption.Keyword)
											   );
			}

            // 設定排序、分頁
			queryOption.SetSource(query);

			return View(queryOption);
		}

		// GET: Articles/Details/5
		public ActionResult Details(Guid? id)
		{
            //防呆
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

            //查詢資料
			Article article = db.Articles.Find(id);
			if (article == null)
			{
				return HttpNotFound();
			}

            //返回View
			return View(article);
		}

		// GET: Articles/Create
		public ActionResult Create()
		{
			ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name");
			return View();
		}

		// POST: Articles/Create
		// 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
		// 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Create(Article article, HttpPostedFileBase[] uploads)
		{
            //檢查副檔名
			CheckFiles(uploads);

            //資料驗證
			if (ModelState.IsValid)
			{
				article.ID = Guid.NewGuid();
				article.CreateUser = WebSiteHelper.CurrentUserID;
                article.UpdateUser = WebSiteHelper.CurrentUserID;
				//更新時間
				article.CreateDate = DateTime.Now;
				article.UpdateDate = DateTime.Now;

				db.Articles.Add(article);

                //上傳圖片資料
                HandleFiles(article, uploads);

                //更新資料庫
				db.SaveChanges();

                //返回頁面
				return RedirectToAction("Index");
			}

			ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", article.CategoryID);
			return View(article);
		}

		// GET: Articles/Edit/5
		public ActionResult Edit(Guid? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Article article = db.Articles.Find(id);
			if (article == null)
			{
				return HttpNotFound();
			}
			ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", article.CategoryID);
			return View(article);
		}

		// POST: Articles/Edit/5
		// 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
		// 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
		[HttpPost]
		[ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult Edit(Article article, HttpPostedFileBase[] uploads)
		{
            //檢查副檔名
			CheckFiles(uploads);

            //驗證更新資料
			if (ModelState.IsValid)
			{
				var instance = db.Articles.FirstOrDefault(x => x.ID == article.ID);

				instance.CategoryID = article.CategoryID;
				instance.Subject = article.Subject;
				instance.Summary = article.Summary;
				instance.ContentText = article.ContentText;
				instance.PublishDate = article.PublishDate;
				instance.IsPublish = article.IsPublish;
				instance.UpdateUser = WebSiteHelper.CurrentUserID;
				instance.UpdateDate = DateTime.Now;

				db.Entry(instance).State = EntityState.Modified;
                //處理圖片
                HandleFiles(article, uploads);

                //更新資料庫
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", article.CategoryID);
			return View(article);
		}

		// GET: Articles/Delete/5
		public ActionResult Delete(Guid? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Article article = db.Articles.Find(id);
			if (article == null)
			{
				return HttpNotFound();
			}
			return View(article);
		}

		// POST: Articles/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(Guid id)
		{
			Article article = db.Articles.Find(id);

            //刪除圖片
            var photos = article.Photos.ToList();
            foreach (var photo in photos)
            {
                this.DeletePhoto(photo.ID);
            }

            //移除文章
			db.Articles.Remove(article);

            //更新資料庫
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}

        /// <summary>
        /// 檢查副檔名
        /// </summary>
        /// <param name="uploads">上傳檔案</param>
		private void CheckFiles(HttpPostedFileBase[] uploads)
		{
			if (uploads != null)
			{
				for (int i = 0; i < uploads.Length; i++)
				{
					var upload = uploads[i];

					//檢查檔名
					//檢查格式
					if (upload != null && !Regex.IsMatch(upload.FileName, @"\.(jpg|jpeg|gif|png)$"))
					{
						ModelState.AddModelError("Uploads[" + i + "]", "僅可上傳圖片檔");
					}
				}
			}

		}

        /// <summary>
        /// 上傳圖片
        /// </summary>
        /// <param name="article">文章物件</param>
        /// <param name="uploads">上傳檔案陣列</param>
		private void HandleFiles(Article article, HttpPostedFileBase[] uploads)
		{
			var timeStamp = DateTime.Now;

			//for create
			if (article.Photos == null)
			{
				article.Photos = new List<Photo>();
			}

			//逐一處理上傳檔案
			foreach (var upload in uploads)
			{
				if (upload == null)
				{
					continue;
				}

				//相同檔名以上傳記錄
				var photo = article.Photos.FirstOrDefault(x => x.FileName == upload.FileName);

				if (photo == null)
				{
					photo = new Photo
					{
						ID = Guid.NewGuid(),
						ArticleID = article.ID,
						FileName = upload.FileName,
						CreateDate = timeStamp,
						UpdateDate = timeStamp
					};
					db.Photos.Add(photo);
					article.Photos.Add(photo);  //關聯
				}
				else
				{
					photo.UpdateDate = timeStamp;
					db.Entry(photo).State = EntityState.Modified;
				}

				//指定檔案存放位置
				var path = Server.MapPath(string.Concat("~/Uploads/", article.ID, "/"));

				if (!Directory.Exists(path))
				{
                    Directory.CreateDirectory(path);
				}

                upload.SaveAs(Path.Combine(path, photo.FileName));
			}
		}

        /// <summary>
        /// 縮圖
        /// </summary>
        /// <param name="id">圖片ID</param>
        /// <param name="w">寬度</param>
        /// <param name="h">高度</param>
        /// <returns></returns>
        public ActionResult ArticlePhoto(Guid id, int w, int h)
        {
            var photo = db.Photos.FirstOrDefault(x => x.ID == id);
            if (photo == null)
            {
                return Content(string.Concat("http://placehold.it/", w, "x", h));
            }
            var filePath = Server.MapPath(string.Concat(
                "~/Uploads/",
                photo.ArticleID,
                "/",
                photo.FileName));

            var image = new WebImage(filePath).Resize(w, h);

            return File(image.GetBytes(), "image/jpeg");
        }

        /// <summary>
        /// 刪除圖片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeletePhoto(Guid id)
        {
            var photo = db.Photos.FirstOrDefault(x => x.ID == id);

            db.Photos.Remove(photo);
            db.SaveChanges();
            var path = Server.MapPath(string.Concat("~/Uploads/", photo.ArticleID, "/"));

            var filePath = Server.MapPath(string.Concat("~/Uploads/", photo.ArticleID, "/", photo.FileName));

            //刪除檔案
            System.IO.File.Delete(filePath);

            //刪資料夾
            if (Directory.EnumerateFiles(path).Any() == false)
            {
                System.IO.Directory.Delete(path);
            }

            return new EmptyResult();
        }
	}
}
