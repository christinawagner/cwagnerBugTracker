using System.ComponentModel.DataAnnotations;

namespace cwagnerBugTracker.Models
{
    public class HistoryChange
    {
        public int Id { get; set; }
        public string Property { get; set; }
        [Display(Name = "Value Was")]
        public string OldValue { get; set; }
        [Display(Name = "Changed To")]
        public string NewValue { get; set; }
        public int TicketHistoryId { get; set; }

        public virtual TicketHistory TicketHistory { get; set; }
    }
}