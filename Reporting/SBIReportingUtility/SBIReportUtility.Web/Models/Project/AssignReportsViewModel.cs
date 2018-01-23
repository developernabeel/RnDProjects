using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBIReportUtility.Web.Models.Project
{
    public class AssignReportsViewModel
    {
        public int Pfid { get; set; }
        public int ProjectId { get; set; }
        public int ReportId { get; set; }

        public ProjectDetailsViewModel ProjectDetails { get; set; }
    }
}