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
            if (!context.Users.Any(u => u.Email == "admin@demo.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "admin@demo.com",
                    Email = "admin@demo.com",
                    FirstName = Roles.Admin.ToString(),
                    LastName = "Demo",
                }, "Password1!");
            }
            if (!context.Users.Any(u => u.Email == "projectmanager@demo.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "projectmanager@demo.com",
                    Email = "projectmanager@demo.com",
                    FirstName = "Project Manager",
                    LastName = "Demo",
                }, "Password1!");
            }
            if (!context.Users.Any(u => u.Email == "developer@demo.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "developer@demo.com",
                    Email = "developer@demo.com",
                    FirstName = Roles.Developer.ToString(),
                    LastName = "Demo",
                }, "Password1!");
            }
            if (!context.Users.Any(u => u.Email == "submitter@demo.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "submitter@demo.com",
                    Email = "submitter@demo.com",
                    FirstName = Roles.Submitter.ToString(),
                    LastName = "Demo",
                }, "Password1!");
            }
            if (!context.Users.Any(u => u.Email == "super@user.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "super@user.com",
                    Email = "super@user.com",
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
            var admin = userManager.FindByEmail("admin@demo.com").Id;
            userManager.AddToRole(admin, Roles.Admin);
            var projectManager = userManager.FindByEmail("projectManager@demo.com").Id;
            userManager.AddToRole(projectManager, Roles.ProjectManager);
            var developer = userManager.FindByEmail("developer@demo.com").Id;
            userManager.AddToRole(projectManager, Roles.Developer);
            var submitter = userManager.FindByEmail("submitter@demo.com").Id;
            userManager.AddToRole(submitter, Roles.Submitter);
            //superuser *just in case*
            var super = userManager.FindByEmail("super@user.com").Id;
            userManager.AddToRole(super, Roles.Super);

            //ticket priorities
            if (!context.TicketPriorities.Any(p => p.Name == "Low"))
            {
                var priority = new TicketPriority
                {
                    Name = "Low"
                };
                context.TicketPriorities.Add(priority);
            }
            if (!context.TicketPriorities.Any(p => p.Name == "Intermediate"))
            {
                var priority = new TicketPriority
                {
                    Name = "Intermediate"
                };
                context.TicketPriorities.Add(priority);
            }
            if (!context.TicketPriorities.Any(p => p.Name == "High"))
            {
                var priority = new TicketPriority
                {
                    Name = "High"
                };
                context.TicketPriorities.Add(priority);
            }
            if (!context.TicketPriorities.Any(p => p.Name == "Urgent"))
            {
                var priority = new TicketPriority
                {
                    Name = "Urgent"
                };
                context.TicketPriorities.Add(priority);
            }

            //ticket status
            if (!context.TicketStatuses.Any(p => p.Name == "Unassigned"))
            {
                var status = new TicketStatus
                {
                    Name = "Open"
                };
                context.TicketStatuses.Add(status);
            }
            if (!context.TicketStatuses.Any(p => p.Name == "Open"))
            {
                var status = new TicketStatus
                {
                    Name = "Open"
                };
                context.TicketStatuses.Add(status);
            }
            if (!context.TicketStatuses.Any(p => p.Name == "In Progress"))
            {
                var status = new TicketStatus
                {
                    Name = "In Progress"
                };
                context.TicketStatuses.Add(status);
            }
            if (!context.TicketStatuses.Any(p => p.Name == "Complete"))
            {
                var status = new TicketStatus
                {
                    Name = "Complete"
                };
                context.TicketStatuses.Add(status);
            }

            //ticket type
            if (!context.TicketTypes.Any(p => p.Name == "Bug"))
            {
                var ticketType = new TicketType
                {
                    Name = "Bug"
                };
                context.TicketTypes.Add(ticketType);
            }
            if (!context.TicketTypes.Any(p => p.Name == "Visual"))
            {
                var ticketType = new TicketType
                {
                    Name = "Visual"
                };
                context.TicketTypes.Add(ticketType);
            }
            if (!context.TicketTypes.Any(p => p.Name == "User Experience"))
            {
                var ticketType = new TicketType
                {
                    Name = "User Experience"
                };
                context.TicketTypes.Add(ticketType);
            }
            if (!context.TicketTypes.Any(p => p.Name == "Other"))
            {
                var ticketType = new TicketType
                {
                    Name = "Other"
                };
                context.TicketTypes.Add(ticketType);
            }
        }
    }
}
