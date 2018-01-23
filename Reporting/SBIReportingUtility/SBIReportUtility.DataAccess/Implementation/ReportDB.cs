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
    public class ReportDB : DBHelper
    {

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ILog errorLog = LogManager.GetLogger("Error");

        public ReportDB() { }

        public ReportDB(string connectionString = "")
            : base(connectionString)
        {

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ReportModel> GetReportList()
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            List<ReportModel> reportList = null;
            try
            {
                reportList = new List<ReportModel>();
                OracleParameter[] objParams = { };
                DataTable dtRecords = new DataTable();
                dtRecords = ReadRecord(objParams, "SEL_ALL_REPORT");

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        public ReportModel GetReportById(int reportId)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            ReportModel reportModel = null;
            try
            {
                reportModel = new ReportModel();
                OracleParameter[] objParams = { 
                                                new OracleParameter("P_ID", reportId)
                                              };

                DataTable dtRecords = new DataTable();
                dtRecords = ReadRecord(objParams, "GET_REPORT");

                if (dtRecords.Rows.Count > 0)
                {
                    reportModel.Id = dtRecords.Rows[0]["ID"] == DBNull.Value ? 0 : Convert.ToInt32(dtRecords.Rows[0]["ID"].ToString());
                    reportModel.Name = dtRecords.Rows[0]["REPORTNAME"] == DBNull.Value ? "" : dtRecords.Rows[0]["REPORTNAME"].ToString();
                    reportModel.Description = dtRecords.Rows[0]["REPORTDESCRIPTION"] == DBNull.Value ? "" : dtRecords.Rows[0]["REPORTDESCRIPTION"].ToString();
                    reportModel.ProcedureName = dtRecords.Rows[0]["PROCEDURENAME"] == DBNull.Value ? "" : dtRecords.Rows[0]["PROCEDURENAME"].ToString();
                    reportModel.ProjectId = dtRecords.Rows[0]["PROJECTID"] == DBNull.Value ? 0 : Convert.ToInt32(dtRecords.Rows[0]["PROJECTID"].ToString());
                    reportModel.ProjectName = dtRecords.Rows[0]["PROJECTNAME"] == DBNull.Value ? "" : dtRecords.Rows[0]["PROJECTNAME"].ToString();
                    reportModel.ConnectionId = dtRecords.Rows[0]["CONNECTIONID"] == DBNull.Value ? 0 : Convert.ToInt32(dtRecords.Rows[0]["CONNECTIONID"].ToString());
                    reportModel.ConnectionName = dtRecords.Rows[0]["CONNECTIONNAME"] == DBNull.Value ? "" : dtRecords.Rows[0]["CONNECTIONNAME"].ToString();
                    reportModel.IsActive = dtRecords.Rows[0]["ISACTIVE"] == DBNull.Value ? 0 : Convert.ToInt32(dtRecords.Rows[0]["ISACTIVE"].ToString());
                }
                return reportModel;
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
        /// 
        /// </summary>
        /// <param name="reportModel"></param>
        /// <returns></returns>
        public OperationDetails AddEditReport(ReportModel reportModel)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            OperationDetails operationDetails = null;
            try
            {
                operationDetails = new OperationDetails();
                OracleParameter[] objParams = { 
                                                new OracleParameter("P_ID", reportModel.Id),
                                                new OracleParameter("P_NAME", reportModel.Name),
                                                new OracleParameter("P_DESCRIPTION", reportModel.Description),
                                                new OracleParameter("P_PROCEDURE", reportModel.ProcedureName),
                                                new OracleParameter("P_PROJECTID", reportModel.ProjectId),
                                                new OracleParameter("P_CONNECTIONID", reportModel.ConnectionId),
                                                new OracleParameter("P_MODIFIEDBY", reportModel.ModifiedBy)
                                              };

                operationDetails = InsertUpdateRecord(objParams, "ADD_EDIT_REPORT");
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
        /// 
        /// </summary>
        /// <param name="reportModel"></param>
        /// <returns></returns>
        public OperationDetails UpdateReportStatus(ReportModel reportModel)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            OperationDetails operationDetails = null;
            try
            {
                operationDetails = new OperationDetails();
                OracleParameter[] objParams = { 
                                                new OracleParameter("P_ID", reportModel.Id),
                                                new OracleParameter("P_MODIFIEDBY", reportModel.ModifiedBy),
                                              };

                operationDetails = InsertUpdateRecord(objParams, "UPDATE_REPORT_STATUS");
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

        public OperationDetails DeleteReport(ReportModel reportModel)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            OperationDetails operationDetails = null;
            try
            {
                operationDetails = new OperationDetails();
                OracleParameter[] objParams = { 
                                                new OracleParameter("P_ID", reportModel.Id),
                                                new OracleParameter("P_DELETEDBY", reportModel.ModifiedBy),
                                              };

                operationDetails = InsertUpdateRecord(objParams, "DELETE_REPORT");
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

        public DataTable GetReportData(string storedProcedureName)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            try
            {
                OracleParameter[] objParams = { };
                DataTable dtRecords = new DataTable();
                dtRecords = ReadRecord(objParams, storedProcedureName);
                return dtRecords;
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
