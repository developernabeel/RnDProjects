using SBIReportUtility.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBIReportUtility.Web.Controllers
{
    [ReportAuthorize(Permissions = "SuperAdmin, User")]
    public class ErrorController : Controller
    {
        public ActionResult Error()
        {
            if (Request.IsAjaxRequest())
                return Json(new { success = false, message = "An error occurred while processing your request.", aaData = "error" }, JsonRequestBehavior.AllowGet);

            return View();
        }

        public ActionResult HttpError404()
        {
            return View();
        }
    }
}
