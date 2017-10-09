using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cwagnerBugTracker.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; }
        [Display(Name = "Ticket")]
        public int TicketId { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        [Display(Name = "Created By")]
        public string AuthorId { get; set; }
        [Display(Name = "File Name")]
        public string FileName { get; set; }
        
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser Author { get; set; }
        public string LocalFileName { get; internal set; }
    }
}