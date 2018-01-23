using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SBIReportUtility.BusinessLayer;
using SBIReportUtility.Common.General;
using log4net;
using SBIReportUtility.Web.Filters;
using SBIReportUtility.Entities;

namespace SBIReportUtility.Web.Controllers
{
    [ReportAuthorize(Permissions = "SuperAdmin")]
    public class HomeController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly ILog errorLog = LogManager.GetLogger("Error");

        [HttpGet]
        public ActionResult Index()
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            try
            {
                UserModel user = (UserModel)Session["CurrentUser"];
                return View(user);
            }
            catch (Exception exception)
            {
                errorLog.Fatal("Exception " + exception.Message + "\n" + exception.StackTrace);
                throw;
            }
            finally
            {
                log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution end.");
            }
        }
    }
}
