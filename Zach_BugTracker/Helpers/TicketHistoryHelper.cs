using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zach_BugTracker.Models;

namespace Zach_BugTracker.Helpers
{
    public class TicketHistoryHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void RecordHistoricalChanges(Ticket oldTicket, Ticket newTicket)
        {
            if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
            {
                var newHistoryRecord = new TicketHistory
                {
                    Property = "TicketStatusId",
                    OldValue = oldTicket.TicketStatus.StatusName,
                    NewValue = newTicket.TicketStatus.StatusName,
                    UpdatedDate = (DateTime)newTicket.Updated,
                    TicketId = newTicket.Id,
                    UpdaterId = HttpContext.Current.User.Identity.GetUserId()
                };

                db.TicketHistories.Add(newHistoryRecord);
            }

            if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                var newHistoryRecord = new TicketHistory
                {
                    Property = "TicketPriorityId",
                    OldValue = oldTicket.TicketPriority.PriorityName,
                    NewValue = newTicket.TicketPriority.PriorityName,
                    UpdatedDate = (DateTime)newTicket.Updated,
                    TicketId = newTicket.Id,
                    UpdaterId = HttpContext.Current.User.Identity.GetUserId()
                };

                db.TicketHistories.Add(newHistoryRecord);
            }


            if (oldTicket.DeveloperId != newTicket.DeveloperId)
            {
                var newHistoryRecord = new TicketHistory
                {
                    Property = "DeveloperId",
                    OldValue = oldTicket.Developer == null ? "UnAssigned" : oldTicket.Developer.FirstName,
                    NewValue = newTicket.Developer == null ? "UnAssigned" : newTicket.Developer.FirstName,
                    UpdatedDate = (DateTime)newTicket.Updated,
                    TicketId = newTicket.Id,
                    UpdaterId = HttpContext.Current.User.Identity.GetUserId()
                };

                db.TicketHistories.Add(newHistoryRecord);
            }



            db.SaveChanges();
        }
    }

}
