using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MVC_Backend.Models;

namespace MVC_Backend.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Logon()
        {

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logon(LogonViewModel model)
        {
            var db = new WorkshopEntities();
            var memberAccount = db.SystemUsers.Where(a => a.Account.Equals(model.Account)).Select(a => a.Password).SingleOrDefault();

            //驗證身份
            //CooKie
            if (ModelState.IsValid)
            {
                if (memberAccount !=null && memberAccount.Equals(model.Password))
                {

                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                        model.Account,
                        DateTime.Now,
                        DateTime.Now.AddMinutes(30),
                        model.Remember, //將管理者登入的Cookie設定成Session Cookie
                        "role", //role

                        FormsAuthentication.FormsCookiePath); //取得form表單路徑
                    //建立加密的票
                    string encTicket = FormsAuthentication.Encrypt(ticket);
                    //將票加入Cookie
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    if (model.Remember == true)
                    {
                        cookie.Expires = DateTime.Now.AddYears(1);
                    }
                    Response.Cookies.Add(cookie);

                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("LogOnError", "請輸入正確的帳號或密碼");
            }
            return View();
        }

        /// <summary>
        /// 密碼加密
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        private string CryptographyPassword(string password, string salt)
        {
            string cryptographyPassword =
                FormsAuthentication.HashPasswordForStoringInConfigFile(password + salt, "sha1");

            return cryptographyPassword;
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Logon", "Home");
        }
    }
}