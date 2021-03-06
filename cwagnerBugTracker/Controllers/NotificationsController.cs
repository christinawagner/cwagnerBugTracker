﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cwagnerBugTracker.Models;
using Microsoft.AspNet.Identity;

namespace cwagnerBugTracker.Controllers
{
    [Authorize]
    [RequireHttps]
    public class NotificationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Notifications
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var notifications = db.Notifications.Include(n => n.Recipient).Where(n => n.RecipientId == user.Id);
            return View(notifications.ToList());
        }

        public ActionResult NotificationsPartial()
        {
            var userId = User.Identity.GetUserId();
            var notifications = db.Notifications.Include(n => n.Recipient).Where(n => n.RecipientId == userId && n.IsNew == true);
            return PartialView(notifications.OrderByDescending(s => s.Sent).Take(3).ToList());
        }

        //Marking notifications as read
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarkAllRead()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            foreach (var item in user.Notifications.Where(n => n.IsNew == true))
            {
                item.IsNew = false;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult MarkRead(int notificationId)
        {
            var notification = db.Notifications.Find(notificationId);
            notification.IsNew = false;
            db.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
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
