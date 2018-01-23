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
    public class ConnectionDB : DBHelper
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ILog errorLog = LogManager.GetLogger("Error");

        public ConnectionDB() { }

        public ConnectionDB(string connectionString = "")
            : base(connectionString)
        {

        }

        /// <summary>
        /// Gets all the connections which are not deleted.
        /// </summary>
        /// <returns></returns>
        public List<ConnectionModel> GetConnectionList()
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            List<ConnectionModel> connectionList = null;
            try
            {
                connectionList = new List<ConnectionModel>();
                OracleParameter[] objParams = { 
                                                
                                              };

                DataTable dtRecords = new DataTable();
                dtRecords = ReadRecord(objParams, "SEL_ALL_CONNECTIONS");

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
                        connectionModel.Project.ProjectName = row["PROJECTNAME"] == DBNull.Value ? "" : row["PROJECTNAME"].ToString();
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

        /// <summary>
        /// Delete connection by id.
        /// </summary>
        /// <param name="connectionId"></param>
        /// <param name="deletedBy"></param>
        /// <returns></returns>
        public OperationDetails DeleteConnection(int connectionId, int deletedBy)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            OperationDetails operationDetails = null;
            try
            {
                operationDetails = new OperationDetails();
                OracleParameter[] objParams = { 
                                                new OracleParameter("P_ID", connectionId),
                                                new OracleParameter("P_DELETEDBY", deletedBy),
                                              };

                operationDetails = InsertUpdateRecord(objParams, "DELETE_CONNECTION");
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
        /// Get connection by Id.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public ConnectionModel GetConnectionById(int projectId)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            ConnectionModel connectionModel = null;
            try
            {
                connectionModel = new ConnectionModel();
                OracleParameter[] objParams = { 
                                                new OracleParameter("P_ID", projectId)
                                              };

                DataTable dtRecords = new DataTable();
                dtRecords = ReadRecord(objParams, "GET_CONNECTION");

                if (dtRecords.Rows.Count > 0)
                {
                    connectionModel.Id = dtRecords.Rows[0]["ID"] == DBNull.Value ? 0 : Convert.ToInt32(dtRecords.Rows[0]["ID"].ToString());
                    connectionModel.ConnectionName = dtRecords.Rows[0]["CONNECTIONNAME"] == DBNull.Value ? "" : dtRecords.Rows[0]["CONNECTIONNAME"].ToString();
                    connectionModel.ProjectId = dtRecords.Rows[0]["PROJECTID"] == DBNull.Value ? 0 : Convert.ToInt32(dtRecords.Rows[0]["PROJECTID"].ToString());
                    connectionModel.SID = dtRecords.Rows[0]["SID"] == DBNull.Value ? "" : dtRecords.Rows[0]["SID"].ToString();
                    connectionModel.IpAddress = dtRecords.Rows[0]["IPADDRESS"] == DBNull.Value ? "" : dtRecords.Rows[0]["IPADDRESS"].ToString();
                    connectionModel.PortNumber = dtRecords.Rows[0]["PORTNUMBER"] == DBNull.Value ? 0 : Convert.ToInt32(dtRecords.Rows[0]["PORTNUMBER"].ToString());
                    connectionModel.ConnectionUsername = dtRecords.Rows[0]["USERNAME"] == DBNull.Value ? "" : dtRecords.Rows[0]["USERNAME"].ToString();
                    connectionModel.ConnectionPassword = dtRecords.Rows[0]["PASSWORD"] == DBNull.Value ? "" : dtRecords.Rows[0]["PASSWORD"].ToString();
                }
                return connectionModel;
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
        /// Adds or updates connection
        /// </summary>
        /// <param name="connectionModel"></param>
        /// <returns></returns>
        public OperationDetails AddEditConnection(ConnectionModel connectionModel)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            OperationDetails operationDetails = null;
            try
            {
                operationDetails = new OperationDetails();
                OracleParameter[] objParams = { 
                                                new OracleParameter("P_ID", connectionModel.Id),
                                                new OracleParameter("P_NAME", connectionModel.ConnectionName),
                                                new OracleParameter("P_PROJECTID", connectionModel.ProjectId),
                                                new OracleParameter("P_SID", connectionModel.SID),
                                                new OracleParameter("P_IPADDRESS", connectionModel.IpAddress),
                                                new OracleParameter("P_PORTNUMBER", connectionModel.PortNumber),
                                                new OracleParameter("P_USERNAME", connectionModel.ConnectionUsername),
                                                new OracleParameter("P_PASSWORD", connectionModel.ConnectionPassword),
                                                new OracleParameter("P_CREATEDBY", connectionModel.ModifiedBy),
                                                new OracleParameter("P_MODIFIEDBY", connectionModel.ModifiedBy),
                                              };

                operationDetails = InsertUpdateRecord(objParams, "ADD_EDIT_CONNECTION");
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
        /// Runs a sample query to test if database connection is successful.
        /// </summary>
        /// <returns></returns>
        public int TestConnection()
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            try
            {
                return ExecuteCommand("SELECT * FROM DUAL", 1);
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
