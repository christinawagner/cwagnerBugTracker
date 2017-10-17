namespace cwagnerBugTracker.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;
    using cwagnerBugTracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Domain;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(cwagnerBugTracker.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            userManager.UserValidator = new UserValidator<ApplicationUser>(userManager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            if (!context.Roles.Any(r => r.Name == Roles.Admin))
            {
                roleManager.Create(new IdentityRole { Name = Roles.Admin });
            }
            if (!context.Roles.Any(r => r.Name == Roles.ProjectManager))
            {
                roleManager.Create(new IdentityRole { Name = Roles.ProjectManager});
            }
            if (!context.Roles.Any(r => r.Name == Roles.Developer))
            {
                roleManager.Create(new IdentityRole { Name = Roles.Developer});
            }
            if (!context.Roles.Any(r => r.Name == Roles.Submitter))
            {
                roleManager.Create(new IdentityRole { Name = Roles.Submitter});
            }
            if (!context.Roles.Any(r => r.Name == Roles.Super))
            {
                roleManager.Create(new IdentityRole { Name = Roles.Super});
            }

            if (!context.Users.Any(u => u.Email == "cwagner0604@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "cwagner0604@gmail.com",
                    Email = "cwagner0604@gmail.com",
                    FirstName = "Christina",
                    LastName = "Wagner",
                }, "Password1!");
            }
            if (!context.Users.Any(u => u.Email == "mjaang@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "mjaang@coderfoundry.com",
                    Email = "mjaang@coderfoundry.com",
                    FirstName = "Mark",
                    LastName = "Jaang",
                }, "Password1!");
            }
            if (!context.Users.Any(u => u.Email == "rchapman@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "rchapman@coderfoundry.com",
                    Email = "rchapman@coderfoundry.com",
                    FirstName = "Ryan",
                    LastName = "Chapman",
                }, "Password1!");
            }
            if (!context.Users.Any(u => u.Email == "cwagner0604+admin@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "cwagner0604+admin@gmail.com",
                    Email = "cwagner0604+admin@gmail.com",
                    FirstName = Roles.Admin.ToString(),
                    LastName = "Demo",
                }, "Password1!");
            }
            if (!context.Users.Any(u => u.Email == "cwagner0604+projectmanager@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "cwagner0604+projectmanager@gmail.com",
                    Email = "cwagner0604+projectmanager@gmail.com",
                    FirstName = "Project Manager",
                    LastName = "Demo",
                }, "Password1!");
            }
            if (!context.Users.Any(u => u.Email == "cwagner0604+developer@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "cwagner0604+developer@gmail.com",
                    Email = "cwagner0604+developer@gmail.com",
                    FirstName = Roles.Developer.ToString(),
                    LastName = "Demo",
                }, "Password1!");
            }
            if (!context.Users.Any(u => u.Email == "cwagner0604+submitter@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "cwagner0604+submitter@gmail.com",
                    Email = "cwagner0604+submitter@gmail.com",
                    FirstName = Roles.Submitter.ToString(),
                    LastName = "Demo",
                }, "Password1!");
            }
            if (!context.Users.Any(u => u.Email == "cwagner0604+super@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "cwagner0604+super@gmail.com",
                    Email = "cwagner0604+super@gmail.com",
                    FirstName = Roles.Super.ToString(),
                    LastName = "User",
                }, "Password1!");
            }

            var adminUserId = userManager.FindByEmail("cwagner0604@gmail.com").Id;
            userManager.AddToRole(adminUserId, Roles.Admin);
            var adminUserId1 = userManager.FindByEmail("mjaang@coderfoundry.com").Id;
            userManager.AddToRole(adminUserId1, Roles.Admin);
            var adminUserId2 = userManager.FindByEmail("rchapman@coderfoundry.com").Id;
            userManager.AddToRole(adminUserId2, Roles.Admin);
            //demo users
            var admin = userManager.FindByEmail("cwagner0604+admin@gmail.com").Id;
            userManager.AddToRole(admin, Roles.Admin);
            var projectManager = userManager.FindByEmail("cwagner0604+projectmanager@gmail.com").Id;
            userManager.AddToRole(projectManager, Roles.ProjectManager);
            var developer = userManager.FindByEmail("cwagner0604+developer@gmail.com").Id;
            userManager.AddToRole(projectManager, Roles.Developer);
            var submitter = userManager.FindByEmail("cwagner0604+submitter@gmail.com").Id;
            userManager.AddToRole(submitter, Roles.Submitter);
            //superuser *just in case*
            var super = userManager.FindByEmail("cwagner0604+super@gmail.com").Id;
            userManager.AddToRole(super, Roles.Super);
        }
    }
}
