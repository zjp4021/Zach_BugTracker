using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zach_BugTracker.Models;

namespace Zach_BugTracker.Helpers
{
    public class NotificationHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        public void ManageNotifications(Ticket oldTicket, Ticket newTicket)
        {
            var ticketHasBeenAssigned = oldTicket.DeveloperId == null && newTicket.DeveloperId != null;
            var ticketHasBeenUnAssigned = oldTicket.DeveloperId != null && newTicket.DeveloperId == null;
            var ticketHasBeenReAssigned = oldTicket.DeveloperId != null && newTicket.DeveloperId != null && oldTicket.DeveloperId != newTicket.DeveloperId;


            if (ticketHasBeenAssigned)
            {
                AddAssignmentNotification(oldTicket, newTicket);
            }
            else if (ticketHasBeenUnAssigned)
            {
                AddUnAssignmentNotification(oldTicket, newTicket);
            }
            else if (ticketHasBeenReAssigned)
            {
                //AddAssignmentNotification(oldTicket, newTicket);
                AddReAssignmentNotification(oldTicket, newTicket);
            }




            //if (ticketHasBeenAssigned)
            //    AddAssignmentNotification();

            //else if (ticketHasBeenUnAssigned)
            //    AddUnAssignmentNotification();

            //else if (ticketHasBeenReAssigned)
            //    AddReAssignmentNotifictation();
        }

        private void AddAssignmentNotification(Ticket oldTicket, Ticket newTicket)
        {
            var notification = new TicketNotifications
            {
                TicketId = newTicket.Id,
                SenderId = HttpContext.Current.User.Identity.GetUserId(),
                CreatedDate = DateTime.Now,
                IsRead = false,
                RecipientId = newTicket.DeveloperId,
                NotificationBody = $"You have been assigned to Ticket Id {newTicket.Id} on Project {newTicket.Project.ProjectName}. The Ticket Title is {newTicket.Title} and was created on {newTicket.Created} with a priority of {newTicket.TicketPriority.PriorityName}"
            };
            db.TicketNotifications.Add(notification);
            db.SaveChanges();

        }

        private void AddUnAssignmentNotification(Ticket oldTicket, Ticket newTicket)
        {
            var notification = new TicketNotifications
            {
                TicketId = newTicket.Id,
                SenderId = HttpContext.Current.User.Identity.GetUserId(),
                CreatedDate = DateTime.Now,
                IsRead = false,
                RecipientId = newTicket.DeveloperId,
                NotificationBody = $"You have been Unassigned to Ticket Id {newTicket.Id} on Project {newTicket.Project.ProjectName}. The Ticket Title is {newTicket.Title} and was created on {newTicket.Created} with a priority of {newTicket.TicketPriority.PriorityName}"
            };
            db.TicketNotifications.Add(notification);
            db.SaveChanges();
        }

        private void AddReAssignmentNotification(Ticket oldTicket, Ticket newTicket)
        {
            var notification = new TicketNotifications
            {
                TicketId = newTicket.Id,
                SenderId = HttpContext.Current.User.Identity.GetUserId(),
                CreatedDate = DateTime.Now,
                IsRead = false,
                RecipientId = newTicket.DeveloperId,
                NotificationBody = $"You have been Reassigned to Ticket Id {newTicket.Id} on Project {newTicket.Project.ProjectName}. The Ticket Title is {newTicket.Title} and was created on {newTicket.Created} with a priority of {newTicket.TicketPriority.PriorityName}"
            };
            db.TicketNotifications.Add(notification);
            db.SaveChanges();
        }

        public static List<TicketNotifications> GetUnreadNotifications()
        {
            var currentUserId = HttpContext.Current.User.Identity.GetUserId();
            return db.TicketNotifications.Include("Sender").Include("Recipient").Where(t => t.RecipientId == currentUserId && !t.IsRead).ToList();
        }
    }
}