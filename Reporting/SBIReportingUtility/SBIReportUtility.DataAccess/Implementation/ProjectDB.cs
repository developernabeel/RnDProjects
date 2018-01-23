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
    public class ProjectDB : DBHelper
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
        /// Update Project Status
        /// </summary>
        /// <param name="projectModel"></param>
        /// <returns></returns>
        public OperationDetails UpdateProjectStatus(ProjectModel projectModel)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            OperationDetails operationDetails = null;
            try
            {
                operationDetails = new OperationDetails();
                OracleParameter[] objParams = { 
                                                new OracleParameter("P_ID", projectModel.Id),
                                                new OracleParameter("P_MODIFIEDBY", projectModel.ModifiedBy),
                                              };

                operationDetails = InsertUpdateRecord(objParams, "UPDATE_PROJECT_STATUS");
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
        /// Add Edit Project
        /// </summary>
        /// <param name="projectModel"></param>
        /// <returns></returns>
        public OperationDetails AddEditProject(ProjectModel projectModel)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            OperationDetails operationDetails = null;
            try
            {
                operationDetails = new OperationDetails();
                OracleParameter[] objParams = { 
                                                new OracleParameter("P_ID", projectModel.Id),
                                                new OracleParameter("P_NAME", projectModel.ProjectName),
                                                new OracleParameter("P_DESCRIPTION", projectModel.Description),
                                                new OracleParameter("P_MODIFIEDBY", projectModel.ModifiedBy)
                                              };

                operationDetails = InsertUpdateRecord(objParams, "ADD_EDIT_PROJECT");
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
        /// Get Project By Id
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns>Project Details</returns>
        public ProjectModel GetProjectById(int projectId)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            ProjectModel projectModel = null;
            try
            {
                projectModel = new ProjectModel();
                OracleParameter[] objParams = { 
                                                new OracleParameter("P_ID", projectId)
                                              };

                DataTable dtRecords = new DataTable();
                dtRecords = ReadRecord(objParams, "GET_PROJECT");

                if (dtRecords.Rows.Count > 0)
                {
                    projectModel.Id = dtRecords.Rows[0]["ID"] == DBNull.Value ? 0 : Convert.ToInt32(dtRecords.Rows[0]["ID"].ToString());
                    projectModel.ProjectName = dtRecords.Rows[0]["PROJECTNAME"] == DBNull.Value ? "" : dtRecords.Rows[0]["PROJECTNAME"].ToString();
                    projectModel.Description = dtRecords.Rows[0]["PROJECTDESCRIPTION"] == DBNull.Value ? "" : dtRecords.Rows[0]["PROJECTDESCRIPTION"].ToString();
                    projectModel.IsActive = dtRecords.Rows[0]["ISACTIVE"] == DBNull.Value ? 0 : Convert.ToInt32(dtRecords.Rows[0]["ISACTIVE"].ToString());
                    
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

        public List<ReportModel> GetReportsByProject(int projectId)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            List<ReportModel> reportList = null;
            try
            {
                reportList = new List<ReportModel>();
                OracleParameter[] objParams = {
                                                  new OracleParameter("P_PROJECTID", projectId)
                                              };
                DataTable dtRecords = new DataTable();
                dtRecords = ReadRecord(objParams, "GET_REPORTS_BY_PROJECT");

                if (dtRecords.Rows.Count > 0)
                {
                    foreach (DataRow row in dtRecords.Rows)
                    {
                        ReportModel reportModel = new ReportModel();
                        reportModel.Id = row["ID"] == DBNull.Value ? 0 : Convert.ToInt32(row["ID"].ToString());
                        reportModel.Name = row["REPORTNAME"] == DBNull.Value ? "" : row["REPORTNAME"].ToString();
                        reportModel.Description = row["REPORTDESCRIPTION"] == DBNull.Value ? "" : row["REPORTDESCRIPTION"].ToString();
                        reportModel.ProcedureName = row["PROCEDURENAME"] == DBNull.Value ? "" : row["PROCEDURENAME"].ToString();
                        reportModel.ProjectId = row["PROJECTID"] == DBNull.Value ? 0 : Convert.ToInt32(row["PROJECTID"].ToString());
                        reportModel.ProjectName = row["PROJECTNAME"] == DBNull.Value ? "" : row["PROJECTNAME"].ToString();
                        reportModel.ConnectionId = row["CONNECTIONID"] == DBNull.Value ? 0 : Convert.ToInt32(row["CONNECTIONID"].ToString());
                        reportModel.ConnectionName = row["CONNECTIONNAME"] == DBNull.Value ? "" : row["CONNECTIONNAME"].ToString();
                        reportModel.IsActive = row["ISACTIVE"] == DBNull.Value ? 0 : Convert.ToInt32(row["ISACTIVE"].ToString());
                        reportList.Add(reportModel);
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
            return reportList;
        }

        public List<UserModel> GetUsersByProject(int projectId)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            List<UserModel> userList = null;
            try
            {
                userList = new List<UserModel>();
                OracleParameter[] objParams = { 
                                                  new OracleParameter("P_PROJECTID", projectId)
                                              };

                DataTable dtRecords = new DataTable();
                dtRecords = ReadRecord(objParams, "GET_USERS_BY_PROJECT");

                if (dtRecords.Rows.Count > 0)
                {
                    foreach (DataRow row in dtRecords.Rows)
                    {
                        UserModel user = new UserModel();
                        user.Id = row["ID"] == DBNull.Value ? 0 : Convert.ToInt32(row["ID"].ToString());
                        user.Pfid = row["PFINDEXNO"] == DBNull.Value ? 0 : Convert.ToInt32(row["PFINDEXNO"].ToString());
                        user.ProjectMapping.IsProjectAdmin = row["ISPROJECTADMIN"] == DBNull.Value ? 0 : Convert.ToInt32(row["ISPROJECTADMIN"].ToString());
                        user.Name = row["NAME"] == DBNull.Value ? "" : row["NAME"].ToString();
                        user.EmailId = row["EMAIL"] == DBNull.Value ? "" : row["EMAIL"].ToString();
                        user.Designation = row["DESIGNATION"] == DBNull.Value ? "" : row["DESIGNATION"].ToString();
                        user.RoleId = row["ROLEID"] == DBNull.Value ? 0 : Convert.ToInt32(row["ROLEID"].ToString());
                        userList.Add(user);
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

        public ProjectMappingModel GetProjectMappingByUser(int projectId, int pfid)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            ProjectMappingModel projectMapping = null;
            try
            {
                OracleParameter[] objParams = {
                                                  new OracleParameter("P_PROJECTID", projectId),
                                                  new OracleParameter("P_PFID", pfid)
                                              };

                DataTable dtRecords = new DataTable();
                dtRecords = ReadRecord(objParams, "GET_PROJECT_MAP_BY_USER");

                if (dtRecords.Rows.Count > 0)
                {
                    projectMapping = new ProjectMappingModel();
                    projectMapping.Id = dtRecords.Rows[0]["ID"] == DBNull.Value ? 0 : Convert.ToInt32(dtRecords.Rows[0]["ID"].ToString());
                    projectMapping.Pfid = dtRecords.Rows[0]["PFINDEXNO"] == DBNull.Value ? 0 : Convert.ToInt32(dtRecords.Rows[0]["PFINDEXNO"].ToString());
                    projectMapping.ProjectId = dtRecords.Rows[0]["PROJECTID"] == DBNull.Value ? 0 : Convert.ToInt32(dtRecords.Rows[0]["PROJECTID"].ToString());
                    projectMapping.IsProjectAdmin = dtRecords.Rows[0]["ISPROJECTADMIN"] == DBNull.Value ? 0 : Convert.ToInt32(dtRecords.Rows[0]["ISPROJECTADMIN"].ToString());
                    projectMapping.Project.Id = dtRecords.Rows[0]["PROJECTID"] == DBNull.Value ? 0 : Convert.ToInt32(dtRecords.Rows[0]["PROJECTID"].ToString());
                    projectMapping.Project.ProjectName = dtRecords.Rows[0]["PROJECTNAME"] == DBNull.Value ? "" : dtRecords.Rows[0]["PROJECTNAME"].ToString();
                    projectMapping.Project.Description = dtRecords.Rows[0]["PROJECTDESCRIPTION"] == DBNull.Value ? "" : dtRecords.Rows[0]["PROJECTDESCRIPTION"].ToString();
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

        public List<ReportModel> GetReportMappingByUser(int projectId, int pfid)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            List<ReportModel> reportList = null;
            try
            {
                reportList = new List<ReportModel>();
                OracleParameter[] objParams = {
                                                  new OracleParameter("P_PROJECTID", projectId),
                                                  new OracleParameter("P_PFID", pfid)
                                              };
                DataTable dtRecords = new DataTable();
                dtRecords = ReadRecord(objParams, "GET_REPORT_MAP_BY_USER");

                if (dtRecords.Rows.Count > 0)
                {
                    foreach (DataRow row in dtRecords.Rows)
                    {
                        ReportModel reportModel = new ReportModel();
                        reportModel.Id = row["ID"] == DBNull.Value ? 0 : Convert.ToInt32(row["ID"].ToString());
                        reportModel.Name = row["REPORTNAME"] == DBNull.Value ? "" : row["REPORTNAME"].ToString();
                        reportModel.Description = row["REPORTDESCRIPTION"] == DBNull.Value ? "" : row["REPORTDESCRIPTION"].ToString();
                        reportModel.ProcedureName = row["PROCEDURENAME"] == DBNull.Value ? "" : row["PROCEDURENAME"].ToString();                        
                        reportModel.ConnectionId = row["CONNECTIONID"] == DBNull.Value ? 0 : Convert.ToInt32(row["CONNECTIONID"].ToString());
                        reportModel.ConnectionName = row["CONNECTIONNAME"] == DBNull.Value ? "" : row["CONNECTIONNAME"].ToString();
                        reportList.Add(reportModel);
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
            return reportList;
        }

        public OperationDetails AssignUnassignReport(int pfid, int reportId, int createdBy)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            OperationDetails operationDetails = null;
            try
            {
                operationDetails = new OperationDetails();
                OracleParameter[] objParams = { 
                                                new OracleParameter("P_PFID", pfid),
                                                new OracleParameter("P_REPORTID", reportId),
                                                new OracleParameter("P_CREATEDBY", createdBy)
                                              };

                operationDetails = InsertUpdateRecord(objParams, "ASSIGN_UNASSIGN_REPORT");
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

        public List<ConnectionModel> GetConnectionsByProject(int projectId)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            List<ConnectionModel> connectionList = null;
            try
            {
                connectionList = new List<ConnectionModel>();
                OracleParameter[] objParams = {
                                                  new OracleParameter("P_PROJECTID", projectId),
                                              };
                DataTable dtRecords = new DataTable();
                dtRecords = ReadRecord(objParams, "GET_CONNECTIONS_BY_PROJECT");

                if (dtRecords.Rows.Count > 0)
                {
                    foreach (DataRow row in dtRecords.Rows)
                    {
                        ConnectionModel connectionModel = new ConnectionModel();
                        connectionModel.Id = row["ID"] == DBNull.Value ? 0 : Convert.ToInt32(row["ID"].ToString());
                        connectionModel.ConnectionName = row["CONNECTIONNAME"] == DBNull.Value ? "" : row["CONNECTIONNAME"].ToString();
                        connectionModel.ProjectId = row["PROJECTID"] == DBNull.Value ? 0 : Convert.ToInt32(row["PROJECTID"].ToString());
                        connectionModel.SID = row["SID"] == DBNull.Value ? "" : row["SID"].ToString();
                        connectionModel.IpAddress = row["IPADDRESS"] == DBNull.Value ? "" : row["IPADDRESS"].ToString();
                        connectionModel.PortNumber = row["PORTNUMBER"] == DBNull.Value ? 0 : Convert.ToInt32(row["PORTNUMBER"].ToString());
                        connectionModel.ConnectionUsername = row["USERNAME"] == DBNull.Value ? "" : row["USERNAME"].ToString();
                        connectionModel.ConnectionPassword = row["PASSWORD"] == DBNull.Value ? "" : row["PASSWORD"].ToString();
                        connectionList.Add(connectionModel);
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
            return connectionList;
        }
    }
}
