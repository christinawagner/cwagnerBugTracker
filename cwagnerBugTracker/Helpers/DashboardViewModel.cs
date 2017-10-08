using cwagnerBugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cwagnerBugTracker.Helpers
{
    public class DashboardViewModel
    {
        public List<Project> Projects { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}