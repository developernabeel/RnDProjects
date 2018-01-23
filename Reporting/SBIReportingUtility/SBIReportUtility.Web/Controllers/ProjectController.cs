using log4net;
using SBIReportUtility.BusinessLayer.Implementation;
using SBIReportUtility.Common.General;
using SBIReportUtility.DataAccess.Implementation;
using SBIReportUtility.Entities;
using SBIReportUtility.Web.Filters;
using SBIReportUtility.Web.Models.Project;
using SBIReportUtility.Web.Models.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBIReportUtility.Web.Controllers
{
    public class ProjectController : BaseController
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
            List<ProjectModel> projectList = new ProjectBL().GetProjectList();
            Result = new
            {
                aaData = (from project in projectList
                          select new
                          {
                              Id = project.Id,
                              ProjectName = project.ProjectName,
                              Description = project.Description,
                              IsActive = project.IsActive == 1 ? "Active" : "Inactive",
                              Action = ""
                          }
                            ).ToArray()
            };
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ReportAuthorize(Permissions = "SuperAdmin")]
        public JsonResult ActivateDeactivateProject(int projectId)
        {
            OperationDetails operationDetails = new ProjectBL().UpdateProjectStatus(projectId, CurrentUser.Pfid);
            if (operationDetails.OperationStatus == 1)
            {
                return new JsonResult()
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new { result = operationDetails.OperationMessage, success = true }
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

        [HttpGet]
        [ReportAuthorize(Permissions = "SuperAdmin")]
        public ActionResult ShowAddEditPopup(int projectId = 0)
        {
            ProjectViewModel model = new ProjectViewModel();

            if (projectId > 0)
            {
                ProjectModel projectModel = new ProjectBL().GetProjectById(projectId);
                if (projectModel != null)
                {
                    model.Id = projectModel.Id;
                    model.ProjectName = projectModel.ProjectName;
                    model.Description = projectModel.Description;
                    model.IsActive = projectModel.IsActive;
                }
                if (model == null)
                    model = new ProjectViewModel();
            }
            return PartialView("_AddEditProject", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ReportAuthorize(Permissions = "SuperAdmin")]
        public JsonResult Create(ProjectViewModel projectViewModel)
        {
            if (ModelState.IsValid)
            {
                ProjectModel model = new ProjectModel();
                model.Id = projectViewModel.Id;
                model.ProjectName = projectViewModel.ProjectName;
                model.Description = projectViewModel.Description;

                if (projectViewModel.Id > 0)
                    model.ModifiedBy = CurrentUser.Pfid;
                else
                    model.CreatedBy = CurrentUser.Pfid;

                OperationDetails operationDetails = new ProjectBL().AddEditProject(model);

                if (operationDetails.OperationStatus == 1)
                {
                    if (operationDetails.OperationLogId > 0)
                    {
                        return new JsonResult()
                        {
                            JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                            Data = new { result = projectViewModel.Id == 0 ? "Project Added Successfully." : "Project Updated Successfully.", success = true, cssClass = "alert-success" }
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

        [HttpGet]
        [ReportAuthorize(Permissions = "User")]
        public ActionResult Dashboard()
        {
            var projectMappingList = new UserBL().GetProjectMappingByUser(CurrentUser.Pfid);
            List<DashboardViewModel> userProjects = new List<DashboardViewModel>();
            foreach (var projectMapping in projectMappingList)
            {
                userProjects.Add(new DashboardViewModel
                {
                    Id = projectMapping.Project.Id,
                    ProjectName = projectMapping.Project.ProjectName,
                    Description = projectMapping.Project.Description,
                    IsProjectAdmin = projectMapping.IsProjectAdmin == 1
                });
            }

            return View(userProjects);
        }

        [HttpGet]
        [ReportAuthorize(Permissions = "User")]
        public ActionResult Details(int projectId)
        {
            ProjectMappingModel projectMapping;
            bool isAssigned = new ProjectBL().IsProjectAssignedToUser(projectId, CurrentUser.Pfid, out projectMapping);
            if (!isAssigned)
            {
                TempData["ErrorMessage"] = "Project not found";
                return RedirectToAction("Dashboard", "Project");
            }

            ProjectDetailsViewModel model = GetProjectDetailsViewModel(projectMapping);
            return View(model);
        }

        [HttpGet]
        [ReportAuthorize(Permissions = "User")]
        public ActionResult ReportsList(int projectId)
        {
            var Result = new object();
            List<ReportModel> reportList = new ProjectBL().GetReportsListByUser(projectId, CurrentUser);
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
                              Action = ""
                          }
                            ).ToArray()
            };
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ReportAuthorize(Permissions = "User")]
        public ActionResult ProjectUsers(int projectId)
        {
            ProjectMappingModel projectMapping;
            bool isAssigned = new ProjectBL().IsProjectAssignedToUser(projectId, CurrentUser.Pfid, out projectMapping);
            if (!isAssigned)
            {
                TempData["ErrorMessage"] = "Project not found";
                return RedirectToAction("Dashboard", "Project");
            }

            ProjectDetailsViewModel model = GetProjectDetailsViewModel(projectMapping);
            return View(model);
        }

        [HttpGet]
        [ReportAuthorize(Permissions = "User")]
        public ActionResult ProjectUsersList(int projectId)
        {
            var Result = new object();
            List<UserModel> userList = new ProjectBL().GetUsersByProject(projectId, CurrentUser.Pfid);
            Result = new
            {
                aaData = (
                from user in userList
                select new
                {
                    Id = user.Id,
                    Pfid = user.Pfid,
                    Name = user.Name,
                    Email = user.EmailId,
                    Designation = user.Designation,
                    IsAdmin = user.ProjectMapping.IsProjectAdmin == 1,
                    Action = ""
                }
                            ).ToArray()
            };
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ReportAuthorize(Permissions = "User")]
        public ActionResult ShowAddProjectUserPopup(int projectId)
        {
            var model = new ProjectUserViewModel { ProjectId = projectId };
            return PartialView("_AddProjectUser", model);
        }

        [HttpPost]
        [ReportAuthorize(Permissions = "User")]
        public ActionResult SearchUserByPfid(int pfid)
        {
            UserBL userBL = new UserBL();
            UserModel user = userBL.GetUserByPfid(pfid);

            // If user not found in User table, get user from HRMS service.
            if (user == null)
                user = userBL.GetUserFromService(pfid);

            if (user == null)
                return Json(new { success = false, message = "Employee with this PFID does not exist." }, JsonRequestBehavior.AllowGet);

            return Json(new { success = true, user = user }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ReportAuthorize(Permissions = "User")]
        public ActionResult CreateProjectUser(ProjectUserViewModel model)
        {
            UserBL userBL = new UserBL();
            OperationDetails operationDetails;

            UserModel user = userBL.GetUserByPfid(model.Pfid);

            if (user != null)
            {
                if (user.RoleId == (int)EnumHelper.Role.SuperAdmin)
                    return Json(new { success = false, message = "Cannot assign project to admin user." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // If user not found in User table, get user from HRMS service.
                user = userBL.GetUserFromService(model.Pfid);

                if (user == null)
                    return Json(new { success = false, message = "Employee with this PFID does not exist." }, JsonRequestBehavior.AllowGet);

                user.CreatedBy = CurrentUser.Pfid;
                operationDetails = userBL.AddUser(user);

                if (operationDetails.OperationStatus != 1)
                    return Json(new { success = false, message = "Something went wrong!!" }, JsonRequestBehavior.AllowGet);
            }

            ProjectMappingModel projectMapping = new ProjectMappingModel();
            projectMapping.Pfid = model.Pfid;
            projectMapping.ProjectId = model.ProjectId;
            projectMapping.IsProjectAdmin = model.IsAdmin ? 1 : 0;
            projectMapping.CreatedBy = CurrentUser.Pfid;

            operationDetails = userBL.AddProjectMapping(projectMapping);

            if (operationDetails.OperationStatus == 1)
                return Json(new { success = true, message = "User added to project successfully." }, JsonRequestBehavior.AllowGet);

            return Json(new { success = false, message = "Something went wrong!!" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ReportAuthorize(Permissions = "User")]
        public ActionResult AssignReports(int projectId, int pfid)
        {
            ProjectMappingModel projectMapping;
            bool isAssigned = new ProjectBL().IsProjectAssignedToUser(projectId, CurrentUser.Pfid, out projectMapping);
            if (!isAssigned)
            {
                TempData["ErrorMessage"] = "Project not found";
                return RedirectToAction("Dashboard", "Project");
            }

            AssignReportsViewModel model = new AssignReportsViewModel
            {
                Pfid = pfid,
                ProjectDetails = GetProjectDetailsViewModel(projectMapping)
            };

            return View(model);
        }

        [HttpGet]
        [ReportAuthorize(Permissions = "User")]
        public ActionResult AssignReportsList(int projectId, int pfid)
        {
            var Result = new object();

            using (ProjectDB projectDB = new ProjectDB())
            {
                List<ReportModel> reportList = projectDB.GetReportsByProject(projectId).Where(p => p.IsActive == 1).ToList();
                List<ReportModel> reportsMapped = projectDB.GetReportMappingByUser(projectId, pfid);
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
                                  IsAssigned = reportsMapped.FirstOrDefault(rm => rm.Id == report.Id) != null,
                                  Action = ""
                              }
                                ).ToArray()
                };
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ReportAuthorize(Permissions = "User")]
        public JsonResult AssignUnassignReport(AssignReportsViewModel model)
        {
            using (ProjectDB projectDB = new ProjectDB())
            {
                OperationDetails operationDetails = projectDB.AssignUnassignReport(model.Pfid, model.ReportId, CurrentUser.Pfid);
                if (operationDetails.OperationStatus == 1)
                    return Json(new { success = true, message = operationDetails.OperationMessage }, JsonRequestBehavior.AllowGet);

                return Json(new { success = false, message = "Something went wrong!!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [ReportAuthorize(Permissions = "User")]
        public ActionResult ProjectConnections(int projectId)
        {
            ProjectMappingModel projectMapping;
            bool isAssigned = new ProjectBL().IsProjectAssignedToUser(projectId, CurrentUser.Pfid, out projectMapping);
            if (!isAssigned)
            {
                TempData["ErrorMessage"] = "Project not found";
                return RedirectToAction("Dashboard", "Project");
            }

            ProjectDetailsViewModel model = GetProjectDetailsViewModel(projectMapping);
            return View(model);
        }

        [HttpGet]
        [ReportAuthorize(Permissions = "User")]
        public ActionResult ProjectConnectionsList(int projectId)
        {
            var Result = new object();
            List<ConnectionModel> connectionList;
            using (ProjectDB projectDB = new ProjectDB())
            {
                connectionList = projectDB.GetConnectionsByProject(projectId);
            }
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
                    }
                            ).ToArray()
            };
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ReportAuthorize(Permissions = "User")]
        public JsonResult DeleteProjectConnection(int connectionId)
        {
            OperationDetails OperationDetails = new ConnectionBL().DeleteConnection(connectionId, CurrentUser.Pfid);
            if (OperationDetails.OperationStatus == 1)
                return Json(new { success = true, message = "Connection deleted successfully." }, JsonRequestBehavior.AllowGet);
            else if (OperationDetails.OperationStatus == 3)
                return Json(new { success = false, message = "Cannot delete connection, it is being used in reports." }, JsonRequestBehavior.AllowGet);

            return Json(new { success = false, message = "Something went wrong!!" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [ReportAuthorize(Permissions = "User")]
        public ActionResult AddEditProjectConnection(int projectId, int connectionId = 0)
        {
            ConnectionViewModel model = new ConnectionViewModel();
            model.ProjectId = projectId;

            ProjectMappingModel projectMapping;
            bool isAssigned = new ProjectBL().IsProjectAssignedToUser(projectId, CurrentUser.Pfid, out projectMapping);
            if (!isAssigned)
            {
                TempData["ErrorMessage"] = "Project not found";
                return RedirectToAction("Dashboard", "Project");
            }

            model.ProjectDetails = GetProjectDetailsViewModel(projectMapping);

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

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ReportAuthorize(Permissions = "User")]
        public ActionResult AddEditProjectConnection(ConnectionViewModel connectionViewModel)
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
            return RedirectToAction("ProjectConnections", "Project", new { projectId = connectionViewModel.ProjectId });
        }

        [HttpGet]
        [ReportAuthorize(Permissions = "User")]
        public ActionResult ProjectReports(int projectId)
        {
            ProjectMappingModel projectMapping;
            bool isAssigned = new ProjectBL().IsProjectAssignedToUser(projectId, CurrentUser.Pfid, out projectMapping);
            if (!isAssigned)
            {
                TempData["ErrorMessage"] = "Project not found";
                return RedirectToAction("Dashboard", "Project");
            }

            ProjectDetailsViewModel model = GetProjectDetailsViewModel(projectMapping);
            return View(model);
        }

        [ReportAuthorize(Permissions = "User")]
        public ActionResult ProjectReportsList(int projectId)
        {
            var Result = new object();

            using (ProjectDB projectDB = new ProjectDB())
            {
                List<ReportModel> reportList = projectDB.GetReportsByProject(projectId);
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
            }
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        [ReportAuthorize(Permissions = "User")]
        public ActionResult ShowAddEditProjectReportPopup(int projectId, int reportId = 0)
        {

            ReportViewModel model = new ReportViewModel();
            ReportBL reportBL = new ReportBL();

            model.ProjectId = projectId;
            List<ConnectionModel> connectionList = new ReportBL().GetConnectionsByProject(projectId);
            var ddlConnectionList = connectionList.Select(a => new { id = a.Id, name = a.ConnectionName });
            model.ConnectionList = new SelectList(ddlConnectionList, "id", "name");

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

                    model.ConnectionList = new SelectList(connectionList, "Id", "CONNECTIONNAME", reportModel.ConnectionId);
                }
                if (model == null)
                    model = new ReportViewModel();

            }
            return PartialView("_AddEditProjectReport", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ReportAuthorize(Permissions = "User")]
        public ActionResult CreateProjectReport(ReportViewModel reportViewModel)
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

        [HttpPost]
        [ReportAuthorize(Permissions = "User")]
        public JsonResult ActivateDeactivateProjectReport(int reportId)
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

        [HttpPost]
        [ReportAuthorize(Permissions = "User")]
        public JsonResult DeleteProjectReport(int reportId)
        {
            OperationDetails OperationDetails = new ReportBL().DeleteReport(reportId, CurrentUser.Pfid);
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

        [NonAction]
        private ProjectDetailsViewModel GetProjectDetailsViewModel(ProjectMappingModel projectMapping)
        {
            return new ProjectDetailsViewModel
            {
                ProjectId = projectMapping.Project.Id,
                ProjectName = projectMapping.Project.ProjectName,
                Description = projectMapping.Project.Description,
                IsProjectAdmin = projectMapping.IsProjectAdmin == 1
            };
        }
    }
}
