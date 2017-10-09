using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cwagnerBugTracker.Models
{
    public class Ticket
    {
        public Ticket()
        {
            Comments = new HashSet<TicketComment>();
            Attachments = new HashSet<TicketAttachment>();
            Histories = new HashSet<TicketHistory>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        [Display(Name = "Project")]
        public int ProjectId { get; set; }
        [Display(Name = "Type")]
        public int TicketTypeId { get; set; }
        [Display(Name = "Priority")]
        public int TicketPriorityId { get; set; }
        [Display(Name = "Status")]
        public int TicketStatusId { get; set; }
        [Display(Name = "Created By")]
        public string CreatedById { get; set; }
        [Display(Name = "Assigned To")]
        public string AssignToUserId { get; set; }

        public virtual Project Project { get; set; }
        public virtual TicketType TicketType { get; set; }
        public virtual TicketPriority TicketPriority { get; set; }
        public virtual TicketStatus TicketStatus { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public virtual ApplicationUser AssignToUser { get; set; }

        public virtual ICollection<TicketComment> Comments { get; set; }
        public virtual ICollection<TicketAttachment> Attachments { get; set; }
        public virtual ICollection<TicketHistory> Histories { get; set; }
    }
}