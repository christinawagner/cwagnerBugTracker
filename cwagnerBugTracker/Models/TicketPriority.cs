using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace cwagnerBugTracker.Models
{
    public class TicketPriority
    {
        public int Id { get; set; }
        [DisplayName("Priority")]
        public string Name { get; set; }
    }
}