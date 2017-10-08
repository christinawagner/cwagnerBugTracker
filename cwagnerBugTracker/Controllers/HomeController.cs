using cwagnerBugTracker.Helpers;
using cwagnerBugTracker.Models;
using Domain;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cwagnerBugTracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectAssignHelper helper = new ProjectAssignHelper();

        public ActionResult Index(string id)
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            var viewModel = new DashboardViewModel()
            {
                Projects = user.Projects.OrderByDescending(p => p.Tickets.OrderByDescending(t => t.Created).Select(s => s.Created).FirstOrDefault()).ToList()
            };

            if(User.IsInRole(Roles.Admin) || User.IsInRole(Roles.ProjectManager))
            {
                viewModel.Tickets = user.Projects.SelectMany(p => p.Tickets).ToList();
            }
            else if(User.IsInRole(Roles.Developer))
            {
                viewModel.Tickets = user.Tickets.ToList();
            }
            else
            {
                var userId = User.Identity.GetUserId();
                viewModel.Tickets = db.Tickets.Where(t => t.CreatedById == userId).ToList();
            }

            return View(viewModel);
        }
    }
}