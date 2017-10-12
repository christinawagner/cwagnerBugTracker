using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cwagnerBugTracker.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsNew { get; set; }
        public DateTimeOffset Sent { get; set; }
        public string RecipientId { get; set; }

        public virtual ApplicationUser Recipient { get; set; }        
    }
}