using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zach_BugTracker.Models
{
    public class TicketType
    {
        public TicketType()
        {
            Tickets = new HashSet<Ticket>();
        }
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }


        //Nav Section
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}