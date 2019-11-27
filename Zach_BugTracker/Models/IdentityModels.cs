using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Zach_BugTracker.Helpers;

namespace Zach_BugTracker.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        public RoleHelper roleHelper = new RoleHelper();

        [Display(Name = "First Name")]
        [StringLength(maximumLength: 20, MinimumLength = 1, ErrorMessage = "First Name must have minimum of 1, and no grater then 20")]
        public string FirstName { get; set; }
        
        [Display(Name = "Last Name")]
        [StringLength(maximumLength: 20, MinimumLength = 1, ErrorMessage = "Lastname Name must have minimum of 1, and no grater then 20")]
        public string LastName { get; set; }
        
        [Display(Name = "Display Name")]
        [StringLength(maximumLength: 20, MinimumLength = 1, ErrorMessage = "Display Name must have minimum of 1, and no grater then 20")] 
        public string DisplayName { get; set; }

        public string AvatarPath { get; set; }

        
        [NotMapped]
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        public string GetUserRole()
        {
            return roleHelper.ListUserRoles(Id).FirstOrDefault();
        }

        public bool IsUserDemo()
        {
            switch (GetUserRole())
            {
                case "DemoAdmin":
                case "DemoPM":
                case "DemoSubmitter":
                case "DemoDeveloper":
                    return true;
                default:
                    return false;
            }
        }



        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<TicketComments> TicketComments { get; set; }
        public virtual ICollection<TicketAttachments> TicketAttachments { get; set; }
        public virtual ICollection<TicketHistory> TicketHistories { get; set; }
        //public virtual ICollection<TicketNotifications> TicketNotifications { get; set; }

        //We will not include an ICollection of Tickets even though 
        //the ticket class has Foreign Keys pointing to it.
        //This is because it will cause problems when creating the relationships...
        //public virtual ICollection<Ticket> Tickets {get; set;}

        public ApplicationUser()
        {
            TicketComments = new HashSet<TicketComments>();
            Projects = new HashSet<Project>();
            TicketAttachments = new HashSet<TicketAttachments>();
            TicketHistories = new HashSet<TicketHistory>();
            //TicketNotifications = new HashSet<TicketNotifications>();

        }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //NEED TO COMMENT OUT THE OVERRIDE SAVE CHANGES IF YOU NEED TO UPDATE DATABASE/////////////////////////////////////////////////////////////////////////////////////////////////// 
        //public override int SaveChanges()
        //{

        //    var userId = HttpContext.Current.User.Identity.GetUserId();
        //    RoleHelper role = new RoleHelper();

        //    if (role.IsDemoUser(userId))
        //    {
        //        //need sweet alert here
        //        return 0;
        //    }

        //    return base.SaveChanges();
        //}
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Zach_BugTracker.Models.Project> Projects { get; set; }

        public System.Data.Entity.DbSet<Zach_BugTracker.Models.Ticket> Tickets { get; set; }

        public System.Data.Entity.DbSet<Zach_BugTracker.Models.TicketPriority> TicketPriorities { get; set; }

        public System.Data.Entity.DbSet<Zach_BugTracker.Models.TicketStatus> TicketStatus { get; set; }

        public System.Data.Entity.DbSet<Zach_BugTracker.Models.TicketType> TicketTypes { get; set; }

        public System.Data.Entity.DbSet<Zach_BugTracker.Models.TicketAttachments> TicketAttachments { get; set; }

        public System.Data.Entity.DbSet<Zach_BugTracker.Models.TicketComments> TicketComments { get; set; }

        public System.Data.Entity.DbSet<Zach_BugTracker.Models.TicketHistory> TicketHistories { get; set; }

        public System.Data.Entity.DbSet<Zach_BugTracker.Models.TicketNotifications> TicketNotifications { get; set; }
    }
}