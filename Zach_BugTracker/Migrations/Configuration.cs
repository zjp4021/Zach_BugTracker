namespace Zach_BugTracker.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Configuration;
    using Zach_BugTracker.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Zach_BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Zach_BugTracker.Models.ApplicationDbContext context)
        {
            #region Role Manager Section

            //Fire up a RoleManager so I can create Roles...
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Project_Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project_Manager" });
            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }
            #endregion


            #region Demo Role Section...Testing this out...
            if (!context.Roles.Any(r => r.Name == "DemoAdmin"))
            {
                roleManager.Create(new IdentityRole { Name = "DemoAdmin" });
            }

            if (!context.Roles.Any(r => r.Name == "DemoPM"))
            {
                roleManager.Create(new IdentityRole { Name = "DemoPM" });
            }

            if (!context.Roles.Any(r => r.Name == "DemoDeveloper"))
            {
                roleManager.Create(new IdentityRole { Name = "DemoDeveloper" });
            }

            if (!context.Roles.Any(r => r.Name == "DemoSubmitter"))
            {
                roleManager.Create(new IdentityRole { Name = "DemoSubmitter" });
            }

            #endregion

            #region User Creation Section

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            var demoPassword = WebConfigurationManager.AppSettings["DemoUserPassword"];

            if (!context.Users.Any(u => u.Email == "ZachPruitt@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "ZachPruitt@Mailinator.com",
                    Email = "ZachPruitt@Mailinator.com",
                    FirstName = "Zach",
                    LastName = "Pruitt",
                    DisplayName = "ZJP",
                    AvatarPath = "/Avatars/Admin.png"
                }, demoPassword);
            }

            if (!context.Users.Any(u => u.Email == "Zion@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Zion@Mailinator.com",
                    Email = "Zion@Mailinator.com",
                    FirstName = "Zion",
                    LastName = "Williamson",
                    DisplayName = "Zion",
                    AvatarPath = "/Avatars/AvatarGuyHeadset.png"
                }, demoPassword);
            }

            if (!context.Users.Any(u => u.Email == "RJ@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "RJ@Mailinator.com",
                    Email = "RJ@Mailinator.com",
                    FirstName = "RJ",
                    LastName = "Barrett",
                    DisplayName = "RJ",
                    AvatarPath = "/Avatars/AvatarGuyBrown.png"
                }, demoPassword);
            }

            if (!context.Users.Any(u => u.Email == "Jerry@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Jerry@Mailinator.com",
                    Email = "Jerry@Mailinator.com",
                    FirstName = "Jerry",
                    LastName = "Stackhouse",
                    DisplayName = "Jerry",
                    AvatarPath = "/Avatars/Submitter-Male.png"
                }, demoPassword);
            }

            if (!context.Users.Any(u => u.Email == "Jimi@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Jimi@Mailinator.com",
                    Email = "Jimi@Mailinator.com",
                    FirstName = "Jimi",
                    LastName = "Hendrix",
                    DisplayName = "Jimi",
                    AvatarPath = "/Avatars/AvatarGuyPinkTie.png"
                }, demoPassword);
            }

            if (!context.Users.Any(u => u.Email == "Derek@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Derek@Mailinator.com",
                    Email = "Derek@Mailinator.com",
                    FirstName = "Derek",
                    LastName = "Jeter",
                    DisplayName = "Derek",
                    AvatarPath = "/Avatars/AvatarGuyRed.png"
                }, demoPassword);
            }

            if (!context.Users.Any(u => u.Email == "Coolest@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "Coolest@Mailinator.com",
                    Email = "Coolest@Mailinator.com",
                    FirstName = "Cool",
                    LastName = "Guy",
                    DisplayName = "Cool",
                    AvatarPath = "/Avatars/AvatarGuyBlue.png"
                }, demoPassword);
            }

            //Seeded Demo Users-----------------------------------------------------------------------------

            if (!context.Users.Any(u => u.Email == "DemoAdmin@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoAdmin@Mailinator.com",
                    Email = "DemoAdmin@Mailinator.com",
                    FirstName = "Demo",
                    LastName = "Admin",
                    DisplayName = "Demo Admin",
                    AvatarPath = "/Avatars/AvatarLady2.png"
                }, demoPassword);
            }

            if (!context.Users.Any(u => u.Email == "DemoPM@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoPM@Mailinator.com",
                    Email = "DemoPM@Mailinator.com",
                    FirstName = "Demo",
                    LastName = "PM",
                    DisplayName = "Demo PM",
                    AvatarPath = "/Avatars/AvatarLady3.png"
                }, demoPassword);
            }

            if (!context.Users.Any(u => u.Email == "DemoDeveloper@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoDeveloper@Mailinator.com",
                    Email = "DemoDeveloper@Mailinator.com",
                    FirstName = "Demo",
                    LastName = "Developer",
                    DisplayName = "Demo Developer",
                    AvatarPath = "/Avatars/AvatarLady1.png"
                }, demoPassword);
            }

            if (!context.Users.Any(u => u.Email == "DemoSubmitter@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "DemoSubmitter@Mailinator.com",
                    Email = "DemoSubmitter@Mailinator.com",
                    FirstName = "Demo",
                    LastName = "Submitter",
                    DisplayName = "Demo Submitter",
                    AvatarPath = "/Avatars/AvatarGuyBlack.png"
                }, demoPassword);
            }
            #endregion

            #region Role Assignment Section
            var userId = userManager.FindByEmail("ZachPruitt@Mailinator.com").Id;
            userManager.AddToRole(userId, "Admin");

            userId = userManager.FindByEmail("DemoAdmin@Mailinator.com").Id;
            userManager.AddToRole(userId, "DemoAdmin");

            userId = userManager.FindByEmail("DemoPM@Mailinator.com").Id;
            userManager.AddToRole(userId, "DemoPM");

            userId = userManager.FindByEmail("DemoDeveloper@Mailinator.com").Id;
            userManager.AddToRole(userId, "DemoDeveloper");

            userId = userManager.FindByEmail("DemoSubmitter@Mailinator.com").Id;
            userManager.AddToRole(userId, "DemoSubmitter");
            
            #endregion

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            #region Section for loading Lookup Data
            //Load up a few more tables

            context.TicketStatus.AddOrUpdate(
                t => t.StatusName,
                    new TicketStatus { StatusName = "Unassigned", Description = "A newly created or simply unassigned Ticket" },
                    new TicketStatus { StatusName = "Assigned", Description = "A Ticket that has been assigned but has yet to be worked on" },
                    new TicketStatus { StatusName = "In Progress", Description = "A Ticket that has been assigned and is currently being worked on" },
                    new TicketStatus { StatusName = "Resolved", Description = "A Ticket that has been completed" },
                    new TicketStatus { StatusName = "Archived", Description = "A Ticket that has been archived" }
                 );

            context.TicketPriorities.AddOrUpdate(
                t => t.PriorityName,
                    new TicketPriority { PriorityName = "Immediate", Description = "This priority level requires completion within 2 days" },
                    new TicketPriority { PriorityName = "Highest", Description = "This priority level requires completion within 1 week" },
                    new TicketPriority { PriorityName = "High", Description = "This priority level requires completion within 2 weeks" },
                    new TicketPriority { PriorityName = "Medium", Description = "This priority level requires completion within 3 weeks" },
                    new TicketPriority { PriorityName = "Low", Description = "This priority level requires completion within 4 weeks" },
                    new TicketPriority { PriorityName = "None", Description = "This priority level does not require any attention" }
                 );


            context.TicketTypes.AddOrUpdate(
                t => t.TypeName,
                    new TicketType { TypeName = "Defect", Description = "A defect in the software has been identified" },
                    new TicketType { TypeName = "Feature Request", Description = "The client has called and requested a new feature be added" },
                    new TicketType { TypeName = "Documentation Request", Description = "The client has called and requested documentation for a specific process" },
                    new TicketType { TypeName = "Training Request", Description = "The client has called requesting training on the software" }
                    );


            context.Projects.AddOrUpdate(
                p => p.ProjectName,
                    new Project { ProjectName = "Zach Blog", Description = "My Blog project that is published on Azure", Created = DateTime.Now },
                    new Project { ProjectName = "Zach Portfolio", Description = "My Portfolio project that is published on Azure", Created = DateTime.Now },
                    new Project { ProjectName = "Zach BugTracker", Description = "My BugTracker project that is published on Azure", Created = DateTime.Now },
                    new Project { ProjectName = "Zach Financial Portal 1", Description = "Part 1 of my Financial Portal project that is published on Azure", Created = DateTime.Now },
                    new Project { ProjectName = "Zach Financial Portal 2", Description = "Part 2 of my Financial Portal project that is published on Azure", Created = DateTime.Now }
                    );
            #endregion

            context.SaveChanges();

            //context.Tickets.AddOrUpdate(
            //    t => t.Title,
            //        new Ticket { ProjectId = context.Projects.FirstOrDefault(p => p.Name == "Zach Blog").Id });

        }
    }
}
