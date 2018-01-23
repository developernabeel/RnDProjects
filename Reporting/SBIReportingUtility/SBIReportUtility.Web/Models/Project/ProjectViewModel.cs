using SBIReportUtility.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SBIReportUtility.Web.Models.Project
{
    public class ProjectViewModel 
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Project Name is required")]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        public int IsActive { get; set; }

        public string Action { get; set; }
    }
}