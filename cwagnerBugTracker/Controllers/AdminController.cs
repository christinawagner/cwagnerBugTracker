using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using cwagnerBugTracker.Helpers;
using cwagnerBugTracker.Models;
using Domain;
using Microsoft.AspNet.Identity;

namespace cwagnerBugTracker.Controllers
{
    [RequireHttps]
    [AuthorizeRoles(Roles.Admin)]
    public class AdminController : Controller
    {
        private UserRoleHelper helper = new UserRoleHelper();
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin
        public ActionResult Index()
        {
            List<AdminUserViewModel> users = new List<AdminUserViewModel>();
            var superId = db.Roles.Single(r => r.Name == Roles.Super).Id;
            var currentUserId = User.Identity.GetUserId();

            foreach (var user in db.Users.Where(u => !u.Roles.Any(r => r.RoleId == superId) && u.Id != currentUserId).ToList())
            {
                var eachUser = new AdminUserViewModel
                {
                    User = user,
                    SelectedRoles = helper.ListUserRoles(user.Id).ToArray()
                };

                users.Add(eachUser);
            }
            return View(users.OrderBy(u => u.User.LastName).ToList());
        }
        
        //GET: Edit
        public ActionResult EditUserRoles(string id)
        {
            if (id == User.Identity.GetUserId())
                return RedirectToAction("Index");

            var user = db.Users.Find(id);
            var helper = new UserRoleHelper();
            var model = new AdminUserViewModel
            {
                User = user,
                SelectedRoles = helper.ListUserRoles(id).ToArray()
            };
            model.Roles = new MultiSelectList(db.Roles.Where(u => u.Name != Roles.Super).ToList(), "Name", "Name", model.SelectedRoles);

            return View(model);
        }

        //POST: Edit
        [HttpPost]
        public ActionResult EditUserRoles(AdminUserViewModel model)
        {
            if (model.User.Id == User.Identity.GetUserId())
                ModelState.AddModelError("InvalidUser", "You may not edit your own role.");

            if(ModelState.IsValid)
            {
                var user = db.Users.Find(model.User.Id);
                UserRoleHelper helper = new UserRoleHelper();

                foreach (var role in db.Roles.Select(r => r.Name).ToList())
                {
                    helper.RemoveUserFromRole(user.Id, role);
                }

                if (model.SelectedRoles != null)
                {
                    foreach (var role in model.SelectedRoles)
                    {
                        helper.AddUserToRole(user.Id, role);
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(model);
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