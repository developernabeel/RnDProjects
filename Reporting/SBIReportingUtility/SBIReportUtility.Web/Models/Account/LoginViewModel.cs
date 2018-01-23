using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace SBIReportUtility.Web.Models.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "PFID is required")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid number")]
        public int Pfid { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}