using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace cwagnerBugTracker.Models
{
    public class TicketStatus
    {
        public int Id { get; set; }
        [DisplayName("Status")]
        public string Name { get; set; }
    }
}