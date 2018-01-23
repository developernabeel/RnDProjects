using log4net;
using Oracle.DataAccess.Client;
using SBIReportUtility.Common.General;
using SBIReportUtility.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBIReportUtility.DataAccess.Implementation
{
    public class UserDB : DBHelper
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
                userList = new List<UserModel>();
                OracleParameter[] objParams = { };

                DataTable dtRecords = new DataTable();
                dtRecords = ReadRecord(objParams, "SEL_ALL_USERS");

                if (dtRecords.Rows.Count > 0)
                {
                    foreach (DataRow row in dtRecords.Rows)
                    {
                        UserModel userModel = new UserModel();
                        userModel.Id = row["ID"] == DBNull.Value ? 0 : Convert.ToInt32(row["ID"].ToString());
                        userModel.Pfid = row["PFINDEXNO"] == DBNull.Value ? 0 : Convert.ToInt32(row["PFINDEXNO"].ToString());
                        userModel.Name = row["NAME"] == DBNull.Value ? "" : row["NAME"].ToString();
                        userModel.EmailId = row["EMAIL"] == DBNull.Value ? "" : row["EMAIL"].ToString();
                        userModel.Designation = row["DESIGNATION"] == DBNull.Value ? "" : row["DESIGNATION"].ToString();
                        userModel.RoleId = row["ROLEID"] == DBNull.Value ? 0 : Convert.ToInt32(row["ROLEID"].ToString());                        
                        userList.Add(userModel);
                    }
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
            return userList;
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
                OracleParameter[] objParams = { 
                                                new OracleParameter("P_PFID", pfid)
                                              };

                DataTable dtRecords = new DataTable();
                dtRecords = ReadRecord(objParams, "GET_USER");

                if (dtRecords.Rows.Count > 0)
                {
                    userModel = new UserModel();
                    userModel.Id = dtRecords.Rows[0]["ID"] == DBNull.Value ? 0 : Convert.ToInt32(dtRecords.Rows[0]["ID"].ToString());
                    userModel.Pfid = dtRecords.Rows[0]["PFINDEXNO"] == DBNull.Value ? 0 : Convert.ToInt32(dtRecords.Rows[0]["PFINDEXNO"].ToString());
                    userModel.Name = dtRecords.Rows[0]["NAME"] == DBNull.Value ? "" : dtRecords.Rows[0]["NAME"].ToString();
                    userModel.EmailId = dtRecords.Rows[0]["EMAIL"] == DBNull.Value ? "" : dtRecords.Rows[0]["EMAIL"].ToString();
                    userModel.Designation = dtRecords.Rows[0]["DESIGNATION"] == DBNull.Value ? "" : dtRecords.Rows[0]["DESIGNATION"].ToString();
                    userModel.RoleId = dtRecords.Rows[0]["ROLEID"] == DBNull.Value ? 0 : Convert.ToInt32(dtRecords.Rows[0]["ROLEID"].ToString());
                }
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
                operationDetails = new OperationDetails();
                OracleParameter[] objParams = { 
                                                new OracleParameter("P_PFID", userModel.Pfid),
                                                new OracleParameter("P_NAME", userModel.Name),
                                                new OracleParameter("P_EMAIL", userModel.EmailId),
                                                new OracleParameter("P_DESIGNATION", userModel.Designation),
                                                new OracleParameter("P_CREATEDBY", userModel.CreatedBy)
                                              };

                operationDetails = InsertUpdateRecord(objParams, "ADD_USER");
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

        /// <summary>
        /// Gets project mapped to the user with mapping and project details.
        /// </summary>
        /// <param name="pfid">PFID of user</param>
        /// <returns>List of project mapping</returns>
        public List<ProjectMappingModel> GetProjectMappingByUser(int pfid)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            List<ProjectMappingModel> projectMapping = null;
            try
            {
                projectMapping = new List<ProjectMappingModel>();
                OracleParameter[] objParams = {
                                                  new OracleParameter("P_PFID", pfid)
                                              };

                DataTable dtRecords = new DataTable();
                dtRecords = ReadRecord(objParams, "GET_PROJECTS_BY_USER");

                if (dtRecords.Rows.Count > 0)
                {
                    foreach (DataRow row in dtRecords.Rows)
                    {
                        ProjectMappingModel projectMappingModel = new ProjectMappingModel();
                        projectMappingModel.Id = row["ID"] == DBNull.Value ? 0 : Convert.ToInt32(row["ID"].ToString());
                        projectMappingModel.Pfid = row["PFINDEXNO"] == DBNull.Value ? 0 : Convert.ToInt32(row["PFINDEXNO"].ToString());
                        projectMappingModel.ProjectId = row["PROJECTID"] == DBNull.Value ? 0 : Convert.ToInt32(row["PROJECTID"].ToString());
                        projectMappingModel.IsProjectAdmin = row["ISPROJECTADMIN"] == DBNull.Value ? 0 : Convert.ToInt32(row["ISPROJECTADMIN"].ToString());
                        projectMappingModel.Project.Id = row["PROJECTID"] == DBNull.Value ? 0 : Convert.ToInt32(row["PROJECTID"].ToString());
                        projectMappingModel.Project.ProjectName = row["PROJECTNAME"] == DBNull.Value ? "" : row["PROJECTNAME"].ToString();
                        projectMappingModel.Project.Description = row["PROJECTDESCRIPTION"] == DBNull.Value ? "" : row["PROJECTDESCRIPTION"].ToString();
                        projectMappingModel.Project.IsActive = row["ISACTIVE"] == DBNull.Value ? 0 : Convert.ToInt32(row["ISACTIVE"].ToString());
                        projectMapping.Add(projectMappingModel);
                    }
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
            return projectMapping;
        }        

        public List<ProjectModel> GetProjectsByUser(int pfid)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            List<ProjectModel> projectList = null;
            try
            {
                projectList = new List<ProjectModel>();
                OracleParameter[] objParams = {
                                                  new OracleParameter("P_PFID", pfid)
                                              };

                DataTable dtRecords = new DataTable();
                dtRecords = ReadRecord(objParams, "GET_PROJECTS_BY_USER");

                if (dtRecords.Rows.Count > 0)
                {
                    foreach (DataRow row in dtRecords.Rows)
                    {
                        ProjectModel projectModel = new ProjectModel();
                        projectModel.Id = row["PROJECTID"] == DBNull.Value ? 0 : Convert.ToInt32(row["PROJECTID"].ToString());                        
                        projectModel.ProjectName = row["PROJECTNAME"] == DBNull.Value ? "" : row["PROJECTNAME"].ToString();
                        projectModel.Description = row["PROJECTDESCRIPTION"] == DBNull.Value ? "" : row["PROJECTDESCRIPTION"].ToString();
                        projectList.Add(projectModel);
                    }
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
            return projectList;
        }

        public List<ProjectModel> GetAllProjects()
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            List<ProjectModel> projectList = null;
            try
            {
                projectList = new List<ProjectModel>();
                OracleParameter[] objParams = {
                                                  
                                              };

                DataTable dtRecords = new DataTable();
                dtRecords = ReadRecord(objParams, "SEL_ALL_PROJECT");

                if (dtRecords.Rows.Count > 0)
                {
                    foreach (DataRow row in dtRecords.Rows)
                    {
                        ProjectModel projectModel = new ProjectModel();
                        projectModel.Id = row["ID"] == DBNull.Value ? 0 : Convert.ToInt32(row["ID"].ToString());
                        projectModel.ProjectName = row["PROJECTNAME"] == DBNull.Value ? "" : row["PROJECTNAME"].ToString();
                        projectModel.Description = row["PROJECTDESCRIPTION"] == DBNull.Value ? "" : row["PROJECTDESCRIPTION"].ToString();
                        projectModel.IsActive = row["ISACTIVE"] == DBNull.Value ? 0 : Convert.ToInt32(row["ISACTIVE"].ToString());
                        projectList.Add(projectModel);
                    }
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
            return projectList;
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
                operationDetails = new OperationDetails();
                OracleParameter[] objParams = { 
                                                new OracleParameter("P_PFID", mappingModel.Pfid),
                                                new OracleParameter("P_PROJECTID", mappingModel.ProjectId),
                                                new OracleParameter("P_ISPROJECTADMIN", mappingModel.IsProjectAdmin),
                                                new OracleParameter("P_CREATEDBY", mappingModel.CreatedBy)
                                              };

                operationDetails = InsertUpdateRecord(objParams, "ADD_PROJECT_MAPPING");
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

        public OperationDetails DeleteProjectMapping(int mappingId, int deletedBy)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            OperationDetails operationDetails = null;
            try
            {
                operationDetails = new OperationDetails();
                OracleParameter[] objParams = { 
                                                new OracleParameter("P_ID", mappingId),
                                                new OracleParameter("P_DELETEDBY", deletedBy)
                                              };

                operationDetails = InsertUpdateRecord(objParams, "DELETE_PROJECT_MAPPING");
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

        public OperationDetails UpdateProjectAdminStatus(int mappingId, int modifiedBy)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            OperationDetails operationDetails = null;
            try
            {
                operationDetails = new OperationDetails();
                OracleParameter[] objParams = { 
                                                new OracleParameter("P_ID", mappingId),
                                                new OracleParameter("P_MODIFIEDBY", modifiedBy)
                                              };

                operationDetails = InsertUpdateRecord(objParams, "UPDATE_PROJECT_MAPPING_ISADMIN");
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
    }
}
