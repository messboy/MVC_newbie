﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_Backend.Models;
using MVC_Backend.ViewModels;
using MVC_Backend.Helpers;

namespace MVC_Backend.Controllers
{
    public class SystemUsersController : Controller
    {
        private WorkshopEntities db = new WorkshopEntities();

        // GET: SystemUsers
        public ActionResult Index(QueryOption<SystemUser> queryOption)
        {
            var query = db.SystemUsers.AsQueryable();

            if (string.IsNullOrEmpty(queryOption.Keyword) == false)
            {
                query = query.Where(x => x.Name.Contains(queryOption.Keyword)
                                        ||
                                        x.Account.Contains(queryOption.Keyword)
                                        ||
                                        x.Email.Contains(queryOption.Keyword));
            }

            queryOption.SetSource(query);

            return View(queryOption);
        }

        // GET: SystemUsers/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemUser systemUser = db.SystemUsers.Find(id);
            if (systemUser == null)
            {
                return HttpNotFound();
            }
            return View(systemUser);
        }

        // GET: SystemUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SystemUsers/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Account,Password,Email,CreateUser,CreateDate,UpdateUser,UpdateDate")] SystemUser systemUser)
        {
            if (ModelState.IsValid)
            {
                systemUser.ID = Guid.NewGuid();


                systemUser.UpdateUser = WebSiteHelper.CurrentUserID;
                systemUser.CreateUser = WebSiteHelper.CurrentUserID;
                systemUser.CreateDate = DateTime.Now;
                systemUser.UpdateDate = DateTime.Now;

                db.SystemUsers.Add(systemUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(systemUser);
        }

        // GET: SystemUsers/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemUser systemUser = db.SystemUsers.Find(id);
            if (systemUser == null)
            {
                return HttpNotFound();
            }
            return View(systemUser);
        }

        // POST: SystemUsers/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Account,Password,Email,CreateUser,CreateDate,UpdateUser,UpdateDate")] SystemUser systemUser)
        {
            if (ModelState.IsValid)
            {
                var target = db.SystemUsers.FirstOrDefault(x => x.ID == systemUser.ID);

                target.UpdateUser = WebSiteHelper.CurrentUserID;
                target.UpdateDate = DateTime.Now;

                db.Entry(target).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(systemUser);
        }

        // GET: SystemUsers/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemUser systemUser = db.SystemUsers.Find(id);
            if (systemUser == null)
            {
                return HttpNotFound();
            }
            return View(systemUser);
        }

        // POST: SystemUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            SystemUser systemUser = db.SystemUsers.Find(id);
            db.SystemUsers.Remove(systemUser);
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
