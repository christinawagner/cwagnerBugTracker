﻿using System;

namespace cwagnerBugTracker.Models
{
    public class TicketHistory
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string Property { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTimeOffset Created { get; set; }
        public string AuthorId { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser Author { get; set; }
    }
}