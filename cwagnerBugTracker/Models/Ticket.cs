using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain;
using System.Linq;

namespace cwagnerBugTracker.Models
{
    public static class EnumExtensions
    {
        public static string GetFriendlyName(this Enum val)
        {
            var displayName = val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).SingleOrDefault() as DisplayAttribute;
            return displayName?.Name ?? val.ToString();
        }

        public static IEnumerable<System.Web.Mvc.SelectListItem> ToSelectList(this Enum val)
        {
            return Enum.GetValues(val.GetType()).Cast<Enum>().Select(x => new System.Web.Mvc.SelectListItem() { Text = x.GetFriendlyName(), Value = x.ToString() });
        }
    }

    public enum TicketPriority
    {
        Low,
        Intermediate,
        High,
        Urgent
    }

    public enum TicketStatus
    {
        Unassigned,
        [Display(Name = "In Progress")]
        InProgress,
        Open,
        Complete
    }

    public enum TicketType
    {
        Bug,
        Visual,
        [Display(Name = "User Experience")]
        UserExperience,
        Other
    }

    public class Ticket
    {
        public Ticket()
        {
            Comments = new HashSet<TicketComment>();
            Attachments = new HashSet<TicketAttachment>();
            Histories = new HashSet<TicketHistory>();
        }

        public int Id { get; set; }
        [Audit]
        public string Title { get; set; }
        [Audit]
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        [Display(Name = "Project")]
        public int ProjectId { get; set; }
        [Display(Name = "Created By")]
        public string CreatedById { get; set; }
        [Display(Name = "Assigned To")]
        [Audit]
        public string AssignToUserId { get; set; }

        [Audit]
        [Display(Name = "Type")]
        public TicketType TicketType { get; set; }
        [Audit]
        [Display(Name = "Priority")]
        public TicketPriority TicketPriority { get; set; }
        [Audit]
        [Display(Name = "Status")]
        public TicketStatus TicketStatus { get; set; }

        public virtual Project Project { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public virtual ApplicationUser AssignToUser { get; set; }

        public virtual ICollection<TicketComment> Comments { get; set; }
        public virtual ICollection<TicketAttachment> Attachments { get; set; }
        public virtual ICollection<TicketHistory> Histories { get; set; }
    }
}