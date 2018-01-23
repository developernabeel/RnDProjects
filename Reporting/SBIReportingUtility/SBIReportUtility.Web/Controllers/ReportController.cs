using log4net;
using SBIReportUtility.BusinessLayer.Implementation;
using SBIReportUtility.Common.General;
using SBIReportUtility.Entities;
using SBIReportUtility.Web.Filters;
using SBIReportUtility.Web.Models.Report;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBIReportUtility.Web.Controllers
{

    public class ReportController : BaseController
    {

        #region Properties

        /// <summary>
        /// The log
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// The error log
        /// </summary>
        private static readonly ILog errorLog = LogManager.GetLogger("Error");

        #endregion

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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ReportAuthorize(Permissions = "SuperAdmin")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ReportAuthorize(Permissions = "SuperAdmin")]
        public ActionResult List()
        {
            var Result = new object();

            List<ReportModel> reportList = new ReportBL().GetReportList();
            Result = new
            {
                aaData = (from report in reportList
                          select new
                          {
                              Id = report.Id,
                              Name = report.Name,
                              Description = report.Description,
                              ProcedureName = report.ProcedureName,
                              ProjectId = report.ProjectId,
                              ProjectName = report.ProjectName,
                              ConnectionId = report.ConnectionId,
                              ConnectionName = report.ConnectionName,
                              IsActive = report.IsActive == 1 ? "Active" : "Inactive",
                              Action = ""
                          }
                            ).ToArray()
            };
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        [ReportAuthorize(Permissions = "SuperAdmin")]
        public ActionResult ShowAddEditPopup(int reportId = 0)
        {

            ReportViewModel model = new ReportViewModel();
            ReportBL reportBL = new ReportBL();

            List<ProjectModel> projectList = new ProjectBL().GetProjectList();
            var ddlProjectList = projectList.Where(i => i.IsActive == 1).Select(a => new { id = a.Id, name = a.ProjectName });
            model.ProjectList = new SelectList(ddlProjectList, "id", "name");

            model.ConnectionList = new SelectList(new List<SelectListItem>());
            if (reportId > 0)
            {
                ReportModel reportModel = reportBL.GetReportById(reportId);
                if (reportModel != null)
                {
                    model.Id = reportModel.Id;
                    model.Name = reportModel.Name;
                    model.Description = reportModel.Description;
                    model.ProcedureName = reportModel.ProcedureName;
                    model.ProjectId = reportModel.ProjectId;
                    model.ProjectName = reportModel.ProjectName;
                    model.ConnectionId = reportModel.ConnectionId;
                    model.IsActive = reportModel.IsActive;

                    List<ConnectionModel> connectionList = reportBL.GetConnectionsByProject(reportModel.ProjectId);
                    model.ConnectionList = new SelectList(connectionList, "Id", "CONNECTIONNAME", reportModel.ConnectionId);
                }
                if (model == null)
                    model = new ReportViewModel();

            }
            return PartialView("_AddEditReport", model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ReportAuthorize(Permissions = "SuperAdmin")]
        public ActionResult Create(ReportViewModel reportViewModel)
        {
            if (ModelState.IsValid)
            {
                ReportModel model = new ReportModel();
                model.ModifiedBy = CurrentUser.Pfid;
                model.Id = reportViewModel.Id;
                model.Name = reportViewModel.Name;
                model.Description = reportViewModel.Description;
                model.ProcedureName = reportViewModel.ProcedureName;
                model.ProjectId = reportViewModel.ProjectId;
                model.ConnectionId = reportViewModel.ConnectionId;

                if (reportViewModel.Id > 0)
                    model.ModifiedBy = CurrentUser.Pfid;
                else
                    model.CreatedBy = CurrentUser.Pfid;

                OperationDetails operationDetails = new ReportBL().AddEditReport(model);

                if (operationDetails.OperationStatus == 1)
                {
                    if (operationDetails.OperationLogId > 0)
                    {
                        return new JsonResult()
                        {
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                            Data = new { result = reportViewModel.Id == 0 ? "Report Added Successfully." : "Report Updated Successfully.", success = true, cssClass = "alert-success" }
                        };
                    }
                    return new JsonResult()
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = new { result = "Already Exists.", success = false, cssClass = "alert-info" }
                    };
                }
            }
            return new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new { result = "Something went wrong!!", success = true, cssClass = "alert-danger" }
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        [HttpPost]
        [ReportAuthorize(Permissions = "SuperAdmin")]
        public JsonResult ActivateDeactivateReport(int reportId)
        {
            OperationDetails OperationDetails = new ReportBL().UpdateReportStatus(reportId, CurrentUser.Pfid);
            if (OperationDetails.OperationStatus == 1)
            {
                return new JsonResult()
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new { result = OperationDetails.OperationMessage, success = true }
                };
            }
            else
            {
                return new JsonResult()
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new { result = "Something went wrong!!", success = false }
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        [HttpPost]
        [ReportAuthorize(Permissions = "SuperAdmin")]
        public JsonResult DeleteReport(int reportId)
        {
            OperationDetails OperationDetails = new ReportBL().DeleteReport(reportId, CurrentUser.Pfid);
            if (OperationDetails.OperationStatus == 1)
            {
                return new JsonResult()
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new { result = "Report deleted successfully.", success = true }
                };
            }
            else
            {
                return new JsonResult()
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new { result = "Something went wrong!!", success = false }
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpPost]
        [ReportAuthorize(Permissions = "SuperAdmin")]
        public JsonResult GetConnectionsByProject(int projectId)
        {
            List<ConnectionModel> connectionList = new ReportBL().GetConnectionsByProject(projectId);
            return Json(new
            {
                success = true,
                connectionList = from connection in connectionList select new { id = connection.Id, text = connection.ConnectionName }
            },
            JsonRequestBehavior.AllowGet);
        }

        [ReportAuthorize(Permissions = "SuperAdmin, User")]
        public FileResult DownloadReport(int reportId)
        {
            ReportBL reportBL = new ReportBL();
            ReportModel report = reportBL.GetReportById(reportId);
            ConnectionModel connection = new ConnectionBL().GetConnectionById(report.ConnectionId);

            DataTable reportData = reportBL.GetReportData(connection, report.ProcedureName);

            byte[] excelFile = SBIReportUtility.Common.ExcelHelper.GetExcelFile(reportData);
            string fileName = report.Name + " - " + DateTime.Now.Ticks.ToString() + ".xlsx";
            return File(excelFile, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}
