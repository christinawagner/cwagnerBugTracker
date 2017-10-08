using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using cwagnerBugTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace cwagnerBugTracker.Helpers
{
    public class ProjectAssignHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> userManager =
            new UserManager<ApplicationUser>(new UserStore<ApplicationUser>
                (new ApplicationDbContext()));

        public bool IsUserOnProject(string userId, int projectId)
        {
            var project = db.Projects.Find(projectId);
            var userBool = project.Users.Any(u => u.Id == userId);
            return (userBool);
        }


        public void AddUserToProject(string userId, int projectId)
        {
            var user = db.Users.Find(userId);
            var project = db.Projects.Find(projectId);
            project.Users.Add(user);
            db.SaveChanges();
        }

        public void RemoveUserFromProject(string userId, int projectId)
        {
            var user = db.Users.Find(userId);
            var project = db.Projects.Find(projectId);
            project.Users.Remove(user);
            db.SaveChanges();
        }

        public List<Project> ListUserProjects (string userId)
        {
            var user = db.Users.Find(userId);
            return user.Projects.OrderByDescending(p => p.Created).ToList();
        }

        public List<Project> MostRecentProjects (string userId)
        {
            var user = db.Users.Find(userId);
            return user.Projects.OrderByDescending(p => p.Created).Take(5).ToList();
        }

        public IList<ApplicationUser> ListUsersOnProject(int projectId)
        {
            var project = db.Projects.Find(projectId);
            var users = project.Users.ToList();
            return users;

        }

        public List<ApplicationUser> ListUsersNotOnProject(int projectId)
        {
            return db.Users.Where(u => u.Projects.All(p => p.Id != projectId)).ToList();
        }
    }
}