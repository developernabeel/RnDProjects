using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SBIReportUtility.Web.Models.Connection
{
    public class ConnectionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Connection Name is required")]
        public string ConnectionName { get; set; }

        [Required(ErrorMessage = "Project is required")]
        public int ProjectId { get; set; }

        public SelectList ProjectList { get; set; }

        [Required(ErrorMessage = "SID is required")]
        public string SID { get; set; }

        [Required(ErrorMessage = "IP Address is required")]
        [RegularExpression(@"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$", ErrorMessage = "Please enter a valid IP address")]
        public string IpAddress { get; set; }

        [Required(ErrorMessage = "Port number is required")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid number")]
        public int PortNumber { get; set; }

        [Required(ErrorMessage = "Connection Username is required")]
        public string ConnectionUsername { get; set; }

        [Required(ErrorMessage = "Connection Password is required")]
        public string ConnectionPassword { get; set; }
    }
}