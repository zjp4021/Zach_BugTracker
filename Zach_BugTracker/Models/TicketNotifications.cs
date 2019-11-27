﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zach_BugTracker.Models
{
    public class TicketNotifications
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string SenderId { get; set; }
        public string RecipientId { get; set; }
       
        public string NotificationBody { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsRead { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser Recipient { get; set; }
        public virtual ApplicationUser Sender { get; set; }

    }
}