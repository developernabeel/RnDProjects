using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBIReportUtility.Web.Models.User
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "PFID is required")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid number")]
        [Display(Name = "PFID")]
        public int Pfid { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Email ID")]
        public string EmailId { get; set; }

        [Display(Name = "Designation")]
        public string Designation { get; set; }
    }
}