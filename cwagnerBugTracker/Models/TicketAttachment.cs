using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace cwagnerBugTracker.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; }
        [DisplayName("Ticket")]
        public int TicketId { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        [DisplayName("Created By")]
        public string AuthorId { get; set; }
        [DisplayName("File Name")]
        public string FileName { get; set; }
        
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser Author { get; set; }
        public string LocalFileName { get; internal set; }
    }
}