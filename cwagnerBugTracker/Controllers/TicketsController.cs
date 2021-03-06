﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cwagnerBugTracker.Models;
using cwagnerBugTracker.Helpers;
using Microsoft.AspNet.Identity;
using Domain;
using System.IO;
using System.Configuration;
using MimeTypes;

namespace cwagnerBugTracker.Controllers
{
    [RequireHttps]
    [Authorize]
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectAssignHelper helper = new ProjectAssignHelper();
        private HistoryHelper historyHelper = new HistoryHelper();
        private NotificationHelper notificationHelper = new NotificationHelper();

        //TICKET COUNTER
        public ActionResult TicketCounter()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var tickets = db.Tickets.Where(t => t.AssignToUserId == user.Id);
            return PartialView("TicketCounterPartial", tickets);
        }

        // GET: All Tickets
        [AuthorizeRoles(Roles.Admin)]
        public ActionResult AllTickets()
        {
            var tickets = db.Tickets.Include(t => t.AssignToUser).Include(t => t.CreatedBy).Include(t => t.Project);
            return View(tickets.ToList());
        }

        public PartialViewResult ListTicketsPartial(List<Ticket> tickets)
        {
            return PartialView(tickets);
        }

        // GET: Tickets
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var tickets = db.Tickets.Include(t => t.AssignToUser).Include(t => t.CreatedBy).Include(t => t.Project);

            if (User.IsInRole(Roles.Admin) || User.IsInRole(Roles.ProjectManager))
            {
                var ticketsInMyProjects = user.Projects.SelectMany(t => t.Tickets);
                foreach (var item in ticketsInMyProjects)
                {
                    return View(tickets.ToList());
                }
            }
            else if (User.IsInRole(Roles.Developer))
            {
                return View(tickets.Where(t => t.AssignToUserId == user.Id).ToList());
            }
            else if (User.IsInRole(Roles.Submitter))
            {
                return View(tickets.Where(t => t.CreatedById == user.Id).ToList());
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole(Roles.Admin)
                || (User.IsInRole(Roles.ProjectManager) && helper.IsUserOnProject(user.Id, ticket.ProjectId))
                || (User.IsInRole(Roles.Developer) && ticket.AssignToUserId == user.Id)
                || (User.IsInRole(Roles.Submitter) && ticket.CreatedById == user.Id))
            {
                return View(ticket);
            }
            return RedirectToAction("Index");
        }

        // GET: Tickets/Create
        [AuthorizeRoles(Roles.Submitter, Roles.Admin)]
        public ActionResult Create(Ticket ticket, int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Projects");
            }
            else
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                ProjectAssignHelper helper = new ProjectAssignHelper();

                //ViewBag.AssignToUserId = new SelectList(db.Users, "Id", "FirstName");
                //ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName");
                //ViewBag.ProjectId = new SelectList(helper.ListUserProjects(User.Identity.GetUserId()), "Id", "Title");
                ViewBag.ProjectId = id;
                ViewBag.TicketPriority = new SelectList(TicketPriority.Low.ToSelectList(), "Value", "Text", TicketPriority.Low.ToString());
                ViewBag.TicketType = new SelectList(TicketType.Bug.ToSelectList(), "Value", "Text", TicketType.Bug);
                return View(ticket);
            }
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(Roles.Submitter, Roles.Admin)]
        public ActionResult Create(Ticket ticket, int id)
        {

            if (ModelState.IsValid)
            {
                ticket.TicketStatus = TicketStatus.Unassigned;
                ticket.CreatedById = User.Identity.GetUserId();
                ticket.Created = DateTimeOffset.UtcNow;
                ticket.ProjectId = id;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.AssignToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignToUserId);
            //ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.ProjectId = new SelectList(helper.ListUserProjects(User.Identity.GetUserId()), "Id", "Title");
            ViewBag.TicketPriority = new SelectList(ticket.TicketPriority.ToSelectList(), ticket.TicketPriority);
            ViewBag.TicketType = new SelectList(ticket.TicketType.ToSelectList(), ticket.TicketType);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            ProjectAssignHelper helper = new ProjectAssignHelper();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            //ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            //ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Title", ticket.ProjectId);
            if (User.IsInRole(Roles.Admin) || User.IsInRole(Roles.ProjectManager))
            {
                ViewBag.TicketStatus = new SelectList(Enum.GetValues(typeof(TicketStatus)), ticket.TicketStatus);
            }
            else
            {
                ViewBag.TicketStatus = new SelectList(new[] { ticket.TicketStatus }, ticket.TicketStatus);
            }

            if (User.IsInRole(Roles.Admin)
                || (User.IsInRole(Roles.ProjectManager) && helper.IsUserOnProject(user.Id, ticket.ProjectId))
                || (User.IsInRole(Roles.Developer) && ticket.AssignToUserId == user.Id)
                || (User.IsInRole(Roles.Submitter) && ticket.CreatedById == user.Id))
            {
                ViewBag.TicketPriority = new SelectList(Enum.GetValues(typeof(TicketPriority)), ticket.TicketPriority);
            }
            else
            {
                ViewBag.TicketPriority = new SelectList(new[] { ticket.TicketPriority }, ticket.TicketPriority);
            }

            var projectUsers = GetAssignableUsers(ticket);
            ViewBag.TicketType = new SelectList(Enum.GetValues(typeof(TicketType)), ticket.TicketType);
            ViewBag.AssignToUserId = new SelectList(projectUsers, "Id", "FirstName", ticket.AssignToUserId);
            return View(ticket);
        }

        private IEnumerable<ApplicationUser> GetAssignableUsers(Ticket ticket)
        {
            var submitterId = db.Roles.Single(r => r.Name == Roles.Submitter).Id;
            var developerId = db.Roles.Single(r => r.Name == Roles.Developer).Id;
            var projectUsers = ticket.Project.Users.Where(u => !u.Roles.Any(r => r.RoleId == submitterId));
            if (User.IsInRole(Roles.ProjectManager))
            {
                projectUsers = projectUsers.Where(u => u.Roles.Any(r => r.RoleId == developerId));
            }
            else if (User.IsInRole(Roles.Developer) || User.IsInRole(Roles.Submitter))
            {
                projectUsers = projectUsers.Where(u => u.Id == ticket.AssignToUserId);
            }

            return (projectUsers);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.Created = ticket.Created;
                ticket.Updated = DateTimeOffset.UtcNow;

                db.Tickets.Attach(ticket);
                //explicitly load the new assigned to user so the name can be used for history changes
                db.Entry(ticket).Reference(p => p.AssignToUser).Load();

                var oldTicket = db.Tickets.AsNoTracking().First(t => t.Id == ticket.Id);
                var ticketHistory = historyHelper.GetHistory(oldTicket, ticket, User.Identity.GetUserId());
                var callbackUrl = Url.Action("Details", "Tickets", new { id = ticket.Id }, protocol: Request.Url.Scheme);

                if (ticketHistory.Changes.Any())
                {
                    ticket.Histories.Add(ticketHistory);
                    if (!String.IsNullOrWhiteSpace(ticket.AssignToUserId))
                    {
                        notificationHelper.Notify(ticket.AssignToUserId, "New Notification From BugTracker", "<a href=\"" + callbackUrl
                            + "\">" + ticket.Title + " has changed.</a>", true);
                    }
                }
                if (oldTicket.AssignToUserId != ticket.AssignToUserId && !String.IsNullOrWhiteSpace(ticket.AssignToUserId))
                {
                    notificationHelper.Notify(ticket.AssignToUserId, "New Notification From BugTracker", "<a href=\"" + callbackUrl
                        + "\">New assignment: " + ticket.Title + "</a>", true);
                    if (!string.IsNullOrWhiteSpace(oldTicket.AssignToUserId))
                    {
                        notificationHelper.Notify(oldTicket.AssignToUserId, "New Notification From BugTracker", "<a href=\"" + callbackUrl
                            + "\">Unassignment from: " + ticket.Title + "</a>", true);
                    }
                }

                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var projectUsers = GetAssignableUsers(ticket);
            ViewBag.AssignToUserId = new SelectList(projectUsers, "Id", "FullName", ticket.AssignToUserId);

            ViewBag.TicketPriority = new SelectList(Enum.GetValues(typeof(TicketPriority)), ticket.TicketPriority);
            ViewBag.TicketStatus = new SelectList(Enum.GetValues(typeof(TicketStatus)), ticket.TicketStatus);
            ViewBag.TicketType = new SelectList(Enum.GetValues(typeof(TicketType)), ticket.TicketType);
            return View(ticket);
        }

        //POST: CREATE COMMENT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment([Bind(Include = "Id,Body,Created,Updated,TicketId,AuthorId")] TicketComment ticketComment)
        {
            if (ModelState.IsValid)
            {
                Ticket ticket = db.Tickets.Find(ticketComment.TicketId);
                var callbackUrl = Url.Action("Details", "Tickets", new { id = ticket.Id }, protocol: Request.Url.Scheme);
                var user = db.Users.Find(User.Identity.GetUserId());
                ticketComment.AuthorId = user.Id;
                ticketComment.Created = DateTimeOffset.UtcNow;
                db.TicketComments.Add(ticketComment);
                db.SaveChanges();
                notificationHelper.Notify(ticket.AssignToUserId, "New Notification From BugTracker", "<a href=\"" + callbackUrl
                            + "\">" + ticket.Title + " has a new comment.</a>", true);
                return RedirectToAction("Details", "Tickets", new { id = ticketComment.TicketId });
            }

            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", ticketComment.AuthorId);
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketComment.TicketId);
            return View(ticketComment);
        }

        //POST: EDIT COMMENT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditComment([Bind(Include = "Id,Body,Created,Updated,TicketId,AuthorId")] TicketComment ticketComment)
        {
            if (ModelState.IsValid)
            {
                Ticket ticket = db.Tickets.Find(ticketComment.TicketId);
                var callbackUrl = Url.Action("Details", "Tickets", new { id = ticket.Id }, protocol: Request.Url.Scheme);
                var oldComment = db.TicketComments.Find(ticketComment.Id);
                oldComment.Updated = DateTimeOffset.UtcNow;
                oldComment.Body = ticketComment.Body;
                db.Entry(oldComment).State = EntityState.Modified;
                db.SaveChanges();
                notificationHelper.Notify(ticket.AssignToUserId, "New Notification From BugTracker", "<a href=\"" + callbackUrl
                            + "\">" + ticket.Title + " has an edited comment.</a>", true);
                return RedirectToAction("Details", "Tickets", new { id = oldComment.TicketId });
            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FullName", ticketComment.AuthorId);
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "Title", ticketComment.TicketId);
            return View(ticketComment);
        }

        //POST: DELETE COMMENT
        [HttpPost, ActionName("DeleteComment")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteComment(int id)
        {
            TicketComment ticketComment = db.TicketComments.Find(id);
            db.TicketComments.Remove(ticketComment);
            db.SaveChanges();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        //FILE UPLOAD
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFile(TicketAttachment attachment, HttpPostedFileBase image)
        {
            if (image == null)
            {
                ModelState.AddModelError("image", "Attachment file is required.");
            }
            else if (image != null && image.ContentLength > 0)
            {
                //check the file extension from the file name to make sure it’s an image/file
                var ext = Path.GetExtension(image.FileName).ToLower();
                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif"
                    && ext != ".pdf" && ext != ".doc" && ext != ".docx" && ext != ".odt" && ext != ".txt")
                {
                    ModelState.AddModelError("image", "Invalid Format.");
                }
            }
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var user = db.Users.Find(User.Identity.GetUserId());
                    var absPath = Server.MapPath(ConfigurationManager.AppSettings["UploadPath"]); // path on physical drive on server
                    attachment.Created = DateTimeOffset.UtcNow;
                    attachment.AuthorId = user.Id;
                    attachment.FileName = image.FileName;
                    attachment.LocalFileName = $"{Guid.NewGuid()}";
                    image.SaveAs(Path.Combine(absPath, attachment.LocalFileName)); //save image
                }
                Ticket ticket = db.Tickets.Find(attachment.TicketId);
                var callbackUrl = Url.Action("Details", "Tickets", new { id = ticket.Id }, protocol: Request.Url.Scheme);
                db.TicketAttachments.Add(attachment);
                db.SaveChanges();
                notificationHelper.Notify(ticket.AssignToUserId, "New Notification From BugTracker", "<a href=\"" + callbackUrl
                            + "\">A file was added to " + ticket.Title + "</a>", true);
                return RedirectToAction("Details", "Tickets", new { id = attachment.TicketId });
            }
            return View(attachment);
        }

        //FILE DOWNLOAD
        public FileContentResult Download(int Id)
        {
            var attachment = db.TicketAttachments.Find(Id);
            var absPath = Server.MapPath(ConfigurationManager.AppSettings["UploadPath"]);
            byte[] fileBytes = GetFile(Path.Combine(absPath, attachment.LocalFileName));
            var mimeType = MimeTypeMap.GetMimeType(Path.GetExtension(attachment.FileName));
            Response.AppendHeader("Content-Disposition", $"inline; filename={attachment.FileName}");
            return File(fileBytes, mimeType);
        }

        byte[] GetFile(string s)
        {
            FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new IOException(s);
            return data;
        }

        //DELETE FILE
        [HttpPost, ActionName("DeleteAttachment")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAttachment(int id)
        {
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            db.TicketAttachments.Remove(ticketAttachment);
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
