using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SBIReportUtility.Web.Models.Project
{
    public class ProjectDetailsViewModel
    {
        public int ProjectId { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        public bool IsProjectAdmin { get; set; }
    }
}