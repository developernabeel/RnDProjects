using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBIReportUtility.Web.Models.Report
{
    public class ReportViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Report Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Procedure Name is required")]
        public string ProcedureName { get; set; }

        [Required(ErrorMessage = "Please select Project")]
        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        [Required(ErrorMessage = "Please select Connection")]
        public int ConnectionId { get; set; }

        public string ConnectionName { get; set; }

        public int IsActive { get; set; }

        public SelectList ProjectList { get; set; }

        public SelectList ConnectionList { get; set; }

    }
}