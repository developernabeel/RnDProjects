using log4net;
using SBIReportUtility.BusinessLayer.Implementation;
using SBIReportUtility.Common.General;
using SBIReportUtility.Common;
using SBIReportUtility.Entities;
using SBIReportUtility.Web.Filters;
using SBIReportUtility.Web.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SBIReportUtility.DataAccess.Implementation;

namespace SBIReportUtility.Web.Controllers
{
    [ReportAuthorize(Permissions = "SuperAdmin")]
    public class UserController : BaseController
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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            var Result = new object();
            List<UserModel> userList = new UserBL().GetUserList();
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
                    Action = ""
                }
                            ).ToArray()
            };
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowAddPopup()
        {
            return PartialView("_AddUser", new UserViewModel());
        }

        [HttpPost]
        public ActionResult GetUserByPfid(int pfid)
        {
            UserModel user = new UserBL().GetUserFromService(pfid);

            if (user != null)
                return Json(new { success = true, user = user }, JsonRequestBehavior.AllowGet);

            return Json(new { success = false, message = "Employee with this PFID does not exist." }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int pfid)
        {
            UserModel user = new UserBL().GetUserFromService(pfid);

            if (user == null)
                return Json(new { success = false, message = "Employee with this PFID does not exist." }, JsonRequestBehavior.AllowGet);

            user.CreatedBy = CurrentUser.Pfid;
            OperationDetails operationDetails = new UserBL().AddUser(user);

            if (operationDetails.OperationStatus == 1)
                return Json(new { success = true, message = "User added successfully." }, JsonRequestBehavior.AllowGet);

            if (operationDetails.OperationStatus == 3)
                return Json(new { success = false, message = "User already exists." }, JsonRequestBehavior.AllowGet);

            return Json(new { success = false, message = "Something went wrong!!" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserProfile(int pfid)
        {
            UserModel user = new UserBL().GetUserByPfid(pfid);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found";
                return RedirectToAction("Index", "User");
            }
            UserViewModel model = new UserViewModel
            {
                Id = user.Id,
                Pfid = user.Pfid,
                Name = user.Name,
                EmailId = user.EmailId,
                Designation = user.Designation
            };
            return View(model);
        }

        public ActionResult ProjectList(int pfid)
        {
            var Result = new object();
            List<ProjectMappingModel> projectMappingList = new UserBL().GetProjectMappingByUser(pfid);
            Result = new
            {
                aaData = (
                from projectMapping in projectMappingList
                select new
                {
                    MappingId = projectMapping.Id,
                    Pfid = projectMapping.Pfid,
                    ProjectId = projectMapping.ProjectId,
                    IsProjectAdmin = projectMapping.IsProjectAdmin,
                    ProjectName = projectMapping.Project.ProjectName,
                    Description = projectMapping.Project.Description,
                    Action = ""
                }
                            ).ToArray()
            };
            return Json(Result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowAssignProjectPopup(int pfid)
        {
            List<ProjectModel> projects = new UserBL().GetProjectsToAssign(pfid, CurrentUser.Pfid, (EnumHelper.Role)CurrentUser.RoleId);

            var model = new AssignProjectViewModel();
            model.ProjectList = new SelectList(projects, "Id", "ProjectName");

            return PartialView("_AssignProject", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignProject(AssignProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                ProjectMappingModel projectMapping = new ProjectMappingModel();
                projectMapping.Pfid = model.Pfid;
                projectMapping.ProjectId = model.ProjectId;
                projectMapping.IsProjectAdmin = model.IsAdmin ? 1 : 0;
                projectMapping.CreatedBy = CurrentUser.Pfid;

                OperationDetails operationDetails = new UserBL().AddProjectMapping(projectMapping);

                if (operationDetails.OperationStatus == 1)
                    return Json(new { success = true, message = "Project assigned successfully." }, JsonRequestBehavior.AllowGet);

                return Json(new { success = false, message = "Something went wrong!!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "Validation Error." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateProjectAdminStatus(int id, int isAdmin)
        {
            using (UserDB userDB = new UserDB())
            {
                OperationDetails operationDetails = userDB.UpdateProjectAdminStatus(id, CurrentUser.Pfid);

                if (operationDetails.OperationStatus == 1)
                    return Json(new { success = true, message = isAdmin == 1 ? "Revoked Admin right successfully." : "Granted Admin right successfully." }, JsonRequestBehavior.AllowGet);

                return Json(new { success = false, message = "Something went wrong!!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UnassignProject(int id)
        {
            using (UserDB userDB = new UserDB())
            {
                OperationDetails operationDetails = userDB.DeleteProjectMapping(id, CurrentUser.Pfid);

                if (operationDetails.OperationStatus == 1)
                    return Json(new { success = true, message = "Project unassigned successfully." }, JsonRequestBehavior.AllowGet);

                return Json(new { success = false, message = "Something went wrong!!" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
