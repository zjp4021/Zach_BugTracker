using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zach_BugTracker.Models;

namespace Zach_BugTracker.Helpers
{
    public class TicketHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RoleHelper roleHelper = new RoleHelper();

        public int SetDefaultTicketStatus()
        {
            return db.TicketStatus.FirstOrDefault(ts => ts.StatusName == "Unassigned").Id;
        }

        //public int CurrentTicketStatus()
        //{
        //    return db.TicketStatus
        //}

        public List<Ticket> ListMyTickets()
        {
            var myTickets = new List<Ticket>();
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            //Step 1: Determine my role
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            
            //Step 2: Use that role to build the appropriate set of Tickets
            //if(myRole == "Admin")
            //{
            //    myTickets.AddRange(db.Tickets);
            //}
            //else if(myRole == "Project_Manager")
            //{
            //    myTickets.AddRange(user.Projects.SelectMany(p => p.Tickets));
            //}
            //else if (myRole == "Developer")
            //{
            //    myTickets.AddRange(db.Tickets.Where(t => t.DeveloperId == userId));
            //}
            //else if (myRole == "Submitter")
            //{
            //    myTickets.AddRange(db.Tickets.Where(t => t.SubmitterId == userId));
            //}


            //An easier way of writing the same code without the else if statements
            //This version uses a Switch statement
            switch (myRole)
            {
                case "Admin":
                case "DemoAdmin":
                    myTickets.AddRange(db.Tickets);
                    break;
                case "Project_Manager":
                case "DemoPM":
                    myTickets.AddRange(user.Projects.SelectMany(p => p.Tickets));
                    break;
                case "Developer":
                case "DemoDeveloper":
                    myTickets.AddRange(db.Tickets.Where(t => t.DeveloperId == userId));
                    break;
                case "Submitter":
                case "DemoSubmitter":
                    myTickets.AddRange(db.Tickets.Where(t => t.SubmitterId == userId));
                    break;
                default:
                    break;
            }

            return myTickets;
        }

    }
}