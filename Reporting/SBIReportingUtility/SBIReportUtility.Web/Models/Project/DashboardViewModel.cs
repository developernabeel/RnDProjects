using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBIReportUtility.Web.Models.Project
{
    public class DashboardViewModel
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public bool IsProjectAdmin { get; set; }
    }
}