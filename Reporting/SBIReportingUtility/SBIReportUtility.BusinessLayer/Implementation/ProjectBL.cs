using log4net;
using SBIReportUtility.Common.General;
using SBIReportUtility.DataAccess.Implementation;
using SBIReportUtility.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBIReportUtility.BusinessLayer.Implementation
{
    public class ProjectBL
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ILog errorLog = LogManager.GetLogger("Error");

        /// <summary>
        /// Get List of Project
        /// </summary>
        /// <returns>Project List</returns>
        public List<ProjectModel> GetProjectList()
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            List<ProjectModel> projectList = null;
            try
            {
                using (ProjectDB projectDB = new ProjectDB())
                {
                    projectList = projectDB.GetProjectList();
                    return projectList; 
                }
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

        /// <summary>
        /// Update Project Status
        /// </summary>
        /// <param name="projectModel"></param>
        /// <returns></returns>
        public OperationDetails UpdateProjectStatus(int projectId, int modifiedBy)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            OperationDetails operationDetails = null;
            try
            {
                ProjectModel projectModel = new ProjectModel();
                projectModel.Id = projectId;
                projectModel.ModifiedBy = modifiedBy;

                using (ProjectDB projectDB = new ProjectDB())
                {
                    operationDetails = projectDB.UpdateProjectStatus(projectModel); 
                }
                return operationDetails;

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

        public OperationDetails AddEditProject(ProjectModel projectModel)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            OperationDetails operationDetails = null;
            try
            {
                using (ProjectDB projectDB = new ProjectDB())
                {
                    operationDetails = projectDB.AddEditProject(projectModel); 
                }
                return operationDetails;

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

        public ProjectModel GetProjectById(int projectId)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            ProjectModel projectModel = null;
            try
            {
                using (ProjectDB projectDB = new ProjectDB())
                {
                    projectModel = projectDB.GetProjectById(projectId); 
                }
                return projectModel;
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

        public bool IsProjectAssignedToUser(int projectId, int pfid, out ProjectMappingModel projectMapping)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            try
            {
                using (ProjectDB projectDB = new ProjectDB())
                {
                    projectMapping = projectDB.GetProjectMappingByUser(projectId, pfid);
                }

                return projectMapping != null;
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

        public List<ReportModel> GetReportsListByUser(int projectId, UserModel loggedinUser)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            try
            {
                using (ProjectDB projectDB = new ProjectDB())
                {
                    var projectMapping = projectDB.GetProjectMappingByUser(projectId, loggedinUser.Pfid);

                    // If no mapping found then user is not assigned the project.
                    if (projectMapping == null)
                        return null;

                    // If user is project admin, then return all reports, else return reports assigned to user.
                    if (projectMapping.IsProjectAdmin == 1)
                        return projectDB.GetReportsByProject(projectId).Where(p => p.IsActive == 1).ToList();

                    return projectDB.GetReportMappingByUser(projectId, loggedinUser.Pfid);
                }
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

        public List<UserModel> GetUsersByProject(int projectId, int loggedinUserPfid)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            List<UserModel> userList = null;
            try
            {
                using (ProjectDB projectDB = new ProjectDB())
                {
                    userList = projectDB.GetUsersByProject(projectId);
                }

                if (userList.Count > 0)
                {
                    var loggedinUser = userList.FirstOrDefault(m => m.Pfid == loggedinUserPfid);
                    if (loggedinUser != null)
                        userList.Remove(loggedinUser);
                }
                return userList;
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
