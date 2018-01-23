using log4net;
using SBIReportUtility.BusinessLayer.Implementation;
using SBIReportUtility.Common.General;
using SBIReportUtility.Entities;
using SBIReportUtility.Web.Filters;
using SBIReportUtility.Web.Models.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBIReportUtility.Web.Controllers
{

    public class ConnectionController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly ILog errorLog = LogManager.GetLogger("Error");

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            log.Debug(filterContext.ActionDescriptor.ActionName + " Method execution start.");
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            log.Debug(filterContext.ActionDescriptor.ActionName + " Method execution end.");
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Exception exception = filterContext.Exception;
            errorLog.Fatal("Exception " + exception.Message + "\n" + exception.StackTrace);
            throw new Exception("An exception has occurred.", exception);
        }

        [HttpGet]
        [ReportAuthorize(Permissions = "SuperAdmin")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [ReportAuthorize(Permissions = "SuperAdmin")]
        public ActionResult List()
        {
            var Result = new object();
            List<ConnectionModel> connectionList = new ConnectionBL().GetConnectionList();
            Result = new
            {
                aaData = (
                from connection in connectionList
                select new
                {
                    Id = connection.Id,
                    ProjectId = connection.ProjectId,
                    ConnectionName = connection.ConnectionName,
                    ProjectName = connection.Project.ProjectName,
                    SID = connection.SID,
                    IpAddress = connection.IpAddress,
                    PortNumber = connection.PortNumber,
                    Action = ""
                }).ToArray()
            };
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ReportAuthorize(Permissions = "SuperAdmin")]
        public JsonResult DeleteConnection(int connectionId)
        {
            OperationDetails OperationDetails = new ConnectionBL().DeleteConnection(connectionId, CurrentUser.Pfid);
            if (OperationDetails.OperationStatus == 1)
                return Json(new { success = true, message = "Connection deleted successfully." }, JsonRequestBehavior.AllowGet);

            return Json(new { success = false, message = "Something went wrong!!" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ReportAuthorize(Permissions = "SuperAdmin")]
        public ActionResult AddEditConnection(int connectionId = 0)
        {
            ConnectionViewModel model = new ConnectionViewModel();
            if (connectionId > 0)
            {
                ConnectionModel connectionModel = new ConnectionBL().GetConnectionById(connectionId);
                if (connectionModel != null)
                {
                    model.Id = connectionModel.Id;
                    model.ConnectionName = connectionModel.ConnectionName;
                    model.ProjectId = connectionModel.ProjectId;
                    model.SID = connectionModel.SID;
                    model.IpAddress = connectionModel.IpAddress;
                    model.PortNumber = connectionModel.PortNumber;
                    model.ConnectionUsername = connectionModel.ConnectionUsername;
                }
            }

            var projects = new ProjectBL().GetProjectList().Where(p => p.IsActive == 1).ToList();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (var project in projects)
            {
                selectListItems.Add(new SelectListItem
                {
                    Text = project.ProjectName,
                    Value = project.Id.ToString(),
                    Selected = model.ProjectId > 0 ? (project.Id == model.ProjectId) : false
                });
            }
            model.ProjectList = new SelectList(selectListItems, "Value", "Text");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ReportAuthorize(Permissions = "SuperAdmin")]
        public ActionResult AddEditConnection(ConnectionViewModel connectionViewModel)
        {
            if (ModelState.IsValid)
            {
                ConnectionModel model = new ConnectionModel();

                model.Id = connectionViewModel.Id;
                model.ConnectionName = connectionViewModel.ConnectionName;
                model.ProjectId = connectionViewModel.ProjectId;
                model.SID = connectionViewModel.SID;
                model.IpAddress = connectionViewModel.IpAddress;
                model.PortNumber = connectionViewModel.PortNumber;
                model.ConnectionUsername = connectionViewModel.ConnectionUsername;
                model.ConnectionPassword = connectionViewModel.ConnectionPassword;

                if (connectionViewModel.Id > 0)
                    model.ModifiedBy = CurrentUser.Pfid;
                else
                    model.CreatedBy = CurrentUser.Pfid;

                OperationDetails operationDetails = new ConnectionBL().AddEditConnection(model);

                if (operationDetails.OperationStatus == 1)
                {
                    if (connectionViewModel.Id == 0)
                        TempData["SuccessMessage"] = "Connection inserted successfully.";
                    else
                        TempData["SuccessMessage"] = "Connection updated successfully.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Validation error.";
            }
            return RedirectToAction("Index", "Connection");
        }

        [HttpPost]
        [ReportAuthorize(Permissions = "SuperAdmin, User")]
        public JsonResult TestConnection(ConnectionViewModel connectionViewModel)
        {
            if (ModelState.IsValid)
            {
                ConnectionModel connection = new ConnectionModel
                {
                    IpAddress = connectionViewModel.IpAddress,
                    PortNumber = connectionViewModel.PortNumber,
                    SID = connectionViewModel.SID,
                    ConnectionUsername = connectionViewModel.ConnectionUsername,
                    ConnectionPassword = connectionViewModel.ConnectionPassword,
                };

                try
                {
                    int result = new ConnectionBL().TestConnection(connection);
                    return Json(new { Success = true, Message = "Database connection tested successfully." });
                }
                catch (Exception)
                {
                    return Json(new { Success = false, Message = "Unable to connect to database." });
                }
            }
            return Json(new { Success = false, Message = "There are validation errors on page." });
        }
    }
}
