using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_Model;


namespace MVC_Practice.Controllers
{
    public class HomeController : Controller
    {
        private WorkShopEntities db = new WorkShopEntities();
        public ActionResult Index()
        {
            //最新五篇文章
            var articles =
                db.Articles.Where(x => x.IsPublish && x.PublishDate <= DateTime.Now)
                  .OrderByDescending(x => x.CreateDate)
                  .Take(5);

            return View(articles.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}