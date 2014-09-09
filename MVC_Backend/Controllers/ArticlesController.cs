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


namespace MVC_Backend.Controllers
{
    public class ArticlesController : Controller
    {
        private WorkshopEntities db = new WorkshopEntities();
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // GET: Articles
        public ActionResult Index(QueryOption<Article> queryOption)
        {
            // alternatively you can call the Log() method 
            // and pass log level as the parameter.
            logger.Log(LogLevel.Info, "Sample informational message");
            var query = db.Articles.Include(a => a.Category);

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

            queryOption.SetSource(query);

            return View(queryOption);
        }

        // GET: Articles/Details/5
        public ActionResult Details(Guid? id)
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
        public ActionResult Create(Article article)
        {
            if (ModelState.IsValid)
            {
                article.ID = Guid.NewGuid();
                //自己新增更新時間
                article.CreateDate = DateTime.Now;
                article.UpdateDate = DateTime.Now;

                db.Articles.Add(article);
                db.SaveChanges();
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
        public ActionResult Edit(Article article)
        {
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
            db.Articles.Remove(article);
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
    }
}
