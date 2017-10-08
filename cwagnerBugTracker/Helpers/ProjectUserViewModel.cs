using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using cwagnerBugTracker.Models;

namespace cwagnerBugTracker.Helpers
{
    public class ProjectUserViewModel
    {
        public Project AssignProject { get; set; }
        public MultiSelectList Users { get; set; }
        public string[] SelectedUsers { get; set; }
    }
}