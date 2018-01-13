using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cwagnerBugTracker.Helpers;
using cwagnerBugTracker.Models;
using Microsoft.AspNet.Identity;
using Domain;

namespace cwagnerBugTracker.Controllers
{
    [RequireHttps]
    [Authorize]
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectAssignHelper helper = new ProjectAssignHelper();

        // GET: Projects
        public ActionResult Index(string id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            ProjectAssignHelper helper = new ProjectAssignHelper();
            var userProjects = helper.ListUserProjects(user.Id);
            return View(userProjects);
        }

        public PartialViewResult ListProjectsPartial(List<Project> projects, bool showArchived = false)
        {
            return PartialView(showArchived ? projects.ToList() : projects.Where(p => !p.Archived).ToList());
        }

        // GET: Project list for all users
        [AuthorizeRoles(Roles.Admin, Roles.ProjectManager)]
        public ActionResult ProjectList()
        {
            return View(db.Projects.ToList());
        }

        //GET: Archived projects
        [AuthorizeRoles(Roles.Admin, Roles.ProjectManager)]
        public ActionResult ArchivedProjects()
        {
            return View(db.Projects.Where(p => p.Archived).ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Include(i => i.Users).SingleOrDefault(s => s.Id == id);
            if (project == null)
            {
                return HttpNotFound();
            }
            var user = db.Users.Find(User.Identity.GetUserId());
            ProjectAssignHelper helper = new ProjectAssignHelper();
            if (helper.IsUserOnProject(user.Id, project.Id) || User.IsInRole(Roles.Admin) || User.IsInRole(Roles.ProjectManager))
            {
            return View(project);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Projects/Create
        [AuthorizeRoles(Roles.Admin, Roles.ProjectManager)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(Roles.Admin, Roles.ProjectManager)]
        public ActionResult Create([Bind(Include = "Id,Created,Updated,Title,Description,AuthorId")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.AuthorId = User.Identity.GetUserId();
                project.Created = DateTimeOffset.UtcNow;
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("ProjectList");
            }

            return View(project);
        }

        // GET: Edit project users
        [AuthorizeRoles(Roles.Admin, Roles.ProjectManager)]
        public ActionResult EditProjectUsers(int? id)
        {
            var project = db.Projects.Find(id);
            var superId = db.Roles.Single(r => r.Name == Roles.Super).Id;
            ProjectUserViewModel projectUserVM = new ProjectUserViewModel
            {
                AssignProject = project,
                SelectedUsers = project.Users.Select(u => u.Id).ToArray()
            };
            projectUserVM.Users = new MultiSelectList(db.Users.Where(u => !u.Roles.Any(r => r.RoleId == superId)).ToList(), "Id", "FullName", projectUserVM.SelectedUsers);

            return View(projectUserVM);
        }

        // POST: Edit project users
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(Roles.Admin, Roles.ProjectManager)]
        public ActionResult EditProjectUsers(ProjectUserViewModel model)
        {
            ProjectAssignHelper helper = new ProjectAssignHelper();
            var project = db.Projects.Find(model.AssignProject.Id);

            foreach (var userId in db.Users.Select(p => p.Id).ToList())
            {
                helper.RemoveUserFromProject(userId, project.Id);
            }

            foreach (var userId in model.SelectedUsers)
            {
                helper.AddUserToProject(userId, project.Id);
            }
            return RedirectToAction("ProjectList");
        }

        //GET: Edit
        [AuthorizeRoles(Roles.Admin, Roles.ProjectManager)]
        public ActionResult Edit(int? id)
        {
            Project project = db.Projects.Find(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        //POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(Roles.Admin, Roles.ProjectManager)]
        public ActionResult Edit(Project project)
        {
            if (ModelState.IsValid)
            {
                project.Created = project.Created;
                project.Updated = DateTimeOffset.UtcNow;
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ProjectList");
            }
            return View(project);
        }

        //// GET: Projects/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Project project = db.Projects.Find(id);
        //    if (project == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(project);
        //}

        // POST: Projects/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Project project = db.Projects.Find(id);
        //    db.Projects.Remove(project);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
