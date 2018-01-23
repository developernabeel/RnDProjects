using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SBIReportUtility.Web.Models.User
{
    public class AssignProjectViewModel
    {
        public int Pfid { get; set; }

        [Required(ErrorMessage = "Please select a project")]
        public int ProjectId { get; set; }

        public SelectList ProjectList { get; set; }

        public bool IsAdmin { get; set; }
    }
}