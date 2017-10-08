using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using cwagnerBugTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace cwagnerBugTracker.Helpers
{
    public class UserRoleHelper
    {
        private UserManager<ApplicationUser> userManager =
            new UserManager<ApplicationUser>(new UserStore<ApplicationUser>
                (new ApplicationDbContext()));
        private ApplicationDbContext db = new ApplicationDbContext();

        //checking if user is in role
        public bool IsUserInRole(string userId, string roleName)
        {
            return userManager.IsInRole(userId, roleName);
        }
        //return list of roles
        public ICollection<string> ListUserRoles(string userId)
        {
            return userManager.GetRoles(userId);
        }
        //add user to role
        public bool AddUserToRole(string userId, string roleName)
        {
            var result = userManager.AddToRole(userId, roleName);
            return result.Succeeded;
        }
        //remove user from frole
        public bool RemoveUserFromRole(string userId, string roleName)
        {
            var result = userManager.RemoveFromRole(userId, roleName);
            return result.Succeeded;
        }
        //return list of users in a specific role
        public ICollection<ApplicationUser> UsersInRole(string roleName)
        {
            List<ApplicationUser> resultList = new List<ApplicationUser>();
            List<ApplicationUser> List = userManager.Users.ToList();
            foreach(var user in List)
            {
                if (IsUserInRole(user.Id, roleName))
                    resultList.Add(user);
            }
            return resultList;
        }
        //return list of users not in a specific role
        public ICollection<ApplicationUser> UsersNotInRole(string roleName)
        {
            List<ApplicationUser> resultList = new List<ApplicationUser>();
            List<ApplicationUser> List = userManager.Users.ToList();
            foreach(var user in List)
            {
                if (!IsUserInRole(user.Id, roleName))
                    resultList.Add(user);
            }
            return resultList;
        }
    }
}