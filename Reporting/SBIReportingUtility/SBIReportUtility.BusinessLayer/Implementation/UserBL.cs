using log4net;
using SBIReportUtility.BusinessLayer.HrmsService;
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
    public class UserBL
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ILog errorLog = LogManager.GetLogger("Error");

        /// <summary>
        /// Get all users from User master table which are not deleted.
        /// </summary>
        /// <returns>List of users</returns>
        public List<UserModel> GetUserList()
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            List<UserModel> userList = null;
            try
            {
                using (UserDB userDB = new UserDB())
                {
                    userList = userDB.GetUserList();
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

        /// <summary>
        /// Gets user by Id.
        /// </summary>
        /// <param name="userId">Id of the user in User master table</param>
        /// <returns>User data</returns>
        public UserModel GetUserByPfid(int pfid)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            UserModel userModel = null;
            try
            {

                using (UserDB userDB = new UserDB())
                {
                    userModel = userDB.GetUserByPfid(pfid);
                    return userModel; 
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
        /// Gets user details by PFID from HRMS service
        /// </summary>
        /// <param name="pfid">Employee PFID</param>
        /// <returns>User data</returns>
        public UserModel GetUserFromService(int pfid)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            UserModel userModel = null;
            try
            {
                SapAccess sapAccess = new SapAccess();
                userModel = sapAccess.GetUserDetails(pfid.ToString());
                return userModel;
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
        /// Adds user to User master table.
        /// </summary>
        /// <param name="userModel">User data</param>
        /// <returns>Database operation details</returns>
        public OperationDetails AddUser(UserModel userModel)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            OperationDetails operationDetails = null;
            try
            {
                using (UserDB userDB = new UserDB())
                {
                    operationDetails = userDB.AddUser(userModel);
                    return operationDetails;
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
        /// Gets a list of project mapped to the user with mapping and project details.
        /// </summary>
        /// <param name="pfid">PFID of user</param>
        /// <returns>List of project mapping</returns>
        public List<ProjectMappingModel> GetProjectMappingByUser(int pfid)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            List<ProjectMappingModel> projectMapping = null;
            try
            {
                using (UserDB userDB = new UserDB())
                {
                    projectMapping = userDB.GetProjectMappingByUser(pfid);
                }
                return projectMapping;
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

        public List<ProjectModel> GetProjectsByUser(int pfid)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            List<ProjectModel> projectList = null;
            try
            {
                using (UserDB userDB = new UserDB())
                {
                    projectList = userDB.GetProjectsByUser(pfid);
                }
                return projectList;
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

        public List<ProjectModel> GetAllProjects()
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            List<ProjectModel> projectList = null;
            try
            {
                using (UserDB userDB = new UserDB())
                {
                    projectList = userDB.GetAllProjects();
                }
                return projectList;
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

        public List<ProjectModel> GetProjectsToAssign(int userPfid, int loggedinUserPfid, EnumHelper.Role loggedinUserRole)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            List<ProjectModel> projectList = new List<ProjectModel>();
            try
            {
                using (UserDB userDB = new UserDB())
                {
                    // Projects already assigned to user.
                    List<ProjectMappingModel> projectsAssigned = userDB.GetProjectMappingByUser(userPfid);

                    // Projects owned by current loggedin user.
                    List<ProjectModel> projectsOwned;

                    // If loggedin user is superadmin, get all projects else get only assigned projects.
                    if (loggedinUserRole == EnumHelper.Role.SuperAdmin)
                        projectsOwned = userDB.GetAllProjects().Where(p => p.IsActive == 1).ToList();
                    else
                        projectsOwned = userDB.GetProjectsByUser(loggedinUserPfid).Where(p => p.IsActive == 1).ToList();

                    foreach (var projectOwned in projectsOwned)
                    {
                        // Only select those projects which are not yet assigned to the user.
                        bool isAlreadyAssigned = projectsAssigned.FirstOrDefault(p => p.ProjectId == projectOwned.Id) != null;
                        if (!isAlreadyAssigned)
                        {
                            projectList.Add(projectOwned);
                        }
                    }
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
        /// Adds mapping to project mapping table
        /// </summary>
        /// <param name="mappingModel">project mapping data</param>
        /// <returns>Database operation details</returns>
        public OperationDetails AddProjectMapping(ProjectMappingModel mappingModel)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            OperationDetails operationDetails = null;
            try
            {
                using (UserDB userDB = new UserDB())
                {
                    operationDetails = userDB.AddProjectMapping(mappingModel);
                    return operationDetails;
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
    }
}
