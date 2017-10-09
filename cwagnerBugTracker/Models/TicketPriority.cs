using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cwagnerBugTracker.Models
{
    public class TicketPriority
    {
        public int Id { get; set; }
        [Display(Name = "Priority")]
        public string Name { get; set; }
    }
}