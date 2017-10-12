using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cwagnerBugTracker.Models
{
    public class TicketHistory
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public DateTimeOffset Created { get; set; }
        [Display(Name = "Change Made By")]
        public string AuthorId { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser Author { get; set; }

        public virtual ICollection<HistoryChange> Changes { get; set; }

        public TicketHistory()
        {
            Changes = new HashSet<HistoryChange>();
        }
    }
}