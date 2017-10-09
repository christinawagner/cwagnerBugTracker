using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cwagnerBugTracker.Models
{
    public class TicketComment
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        [Display(Name = "Ticket")]
        public int TicketId { get; set; }
        [Display(Name = "Created By")]
        public string AuthorId { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser Author { get; set; }
    }
}