using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zach_BugTracker.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
       
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

       
        //Navigation Section
        public virtual ICollection<Ticket> Tickets { get; set;}
        public virtual ICollection<ApplicationUser> Users { get; set; }



        public Project()
        {
            Tickets = new HashSet<Ticket>();
            Users = new HashSet<ApplicationUser>();

        }
    }
}