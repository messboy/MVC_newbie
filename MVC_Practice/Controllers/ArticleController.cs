using MVC_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Web.Helpers;

namespace MVC_Practice.Controllers
{
    public class ArticleController : Controller
    {
        private WorkShopEntities db = new WorkShopEntities();
        private int pageSize = 5;

        /// <summary>
        /// 取得所有類別
        /// </summary>
        public List<Category> Catrgories
        {
            get
            {
                return db.Categories.OrderByDescending(x => x.CreateDate).ToList();
            }
        }

        // GET: Article
        public ActionResult Hot(Guid? categoryID, int page = 1)
        {
            ViewBag.CategoryID = !categoryID.HasValue ? "" : categoryID.ToString();
            ViewBag.ArticleCategories = new SelectList(this.Catrgories, "ID", "Name", categoryID);

            //分頁防呆
            int pageIndex = page < 1 ? 1 : page;

            //顯示發佈時間內的文章
            var articles = db.Articles.Where(x => x.IsPublish && x.PublishDate <= DateTime.Now);

            //類別分類
            if (categoryID.HasValue)
            {
                articles = articles
                        .Where(x => x.CategoryID == categoryID.Value)
                        .OrderByDescending(x => x.ViewCount);               //按照點擊多寡順序排序
            }
            else
            {
                articles = articles.OrderByDescending(x => x.ViewCount);
            }

            //設定分頁
            return View(articles.ToPagedList(pageIndex, pageSize));
        }

        public ActionResult List(Guid? categoryID, int page =1)
        {
            int pageIndex = page < 1 ? 1 : page;

            //文章分類
            ViewBag.CategoryID = !categoryID.HasValue
                                ? ""
                                : categoryID.ToString();

            //所有文章分類
            ViewBag.ArticleCategories =
                new SelectList(this.Catrgories.OrderBy(x => x.CreateDate), "ID", "Name", categoryID);

            //文章
            var articles = db.Articles.Where(x => x.IsPublish && x.PublishDate <= DateTime.Now);
            if (categoryID.HasValue)
            {   //列出該分類文章
                articles = articles
                    .Where(x => x.CategoryID == categoryID.Value)
                    .OrderByDescending(x => x.CreateDate);
            }
            else
            {   //列出所有文章
                articles = articles.OrderByDescending(x => x.CreateDate);
            }
            return View(articles.ToPagedList(pageIndex, pageSize));
        }

        public ActionResult Details(Guid? id)
        {
            if (!id.HasValue)
            { 
                return RedirectToAction("Index");
            }

            var article = db.Articles.FirstOrDefault(x => x.ID == id.Value);
            this.IncreaseViewCount(article);

            return View(article);
        }

        /// <summary>
        /// 增加瀏覽次數
        /// </summary>
        /// <param name="article"></param>
        private void IncreaseViewCount(Article article)
        {
            try
            {
                article.ViewCount += 1;
                db.Entry(article).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

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

    }
}