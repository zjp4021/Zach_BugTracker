using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zach_BugTracker.Models
{
    public class TicketStatus
    {
        public TicketStatus()
        {
            Tickets = new HashSet<Ticket>();
        }
        public int Id { get; set; }
        public string StatusName { get; set; }
        public string Description { get; set; }

        //Nav Section
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}