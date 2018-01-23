using log4net;
using SBIReportUtility.Common.General;
using SBIReportUtility.DataAccess.Implementation;
using SBIReportUtility.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBIReportUtility.BusinessLayer.Implementation
{
    public class ReportBL
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ILog errorLog = LogManager.GetLogger("Error");

        /// <summary>
        /// Get Report List
        /// </summary>
        /// <returns>Report List</returns>
        public List<ReportModel> GetReportList()
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            List<ReportModel> reportList = null;
            try
            {
                using (ReportDB reportDB = new ReportDB())
                {
                    reportList = reportDB.GetReportList(); 
                }
                return reportList;
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
        /// <param name="reportId"></param>
        /// <returns></returns>
        public ReportModel GetReportById(int reportId)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            ReportModel reportModel = null;
            try
            {
                using (ReportDB reportDB = new ReportDB())
                {
                    reportModel = reportDB.GetReportById(reportId); 
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
                using (ReportDB reportDB = new ReportDB())
                {
                    operationDetails = reportDB.AddEditReport(reportModel); 
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="modifiedBy"></param>
        /// <returns></returns>
        public OperationDetails UpdateReportStatus(int reportId, int modifiedBy)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            OperationDetails operationDetails = null;
            try
            {
                ReportModel reportModel = new ReportModel();
                reportModel.Id = reportId;
                reportModel.ModifiedBy = modifiedBy;

                using (ReportDB reportDB = new ReportDB())
                {
                    operationDetails = reportDB.UpdateReportStatus(reportModel); 
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportId"></param>
        /// <param name="deletedBy"></param>
        /// <returns></returns>
        public OperationDetails DeleteReport(int reportId, int deletedBy)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            OperationDetails operationDetails = null;
            try
            {
                ReportModel reportModel = new ReportModel();
                reportModel.Id = reportId;
                reportModel.ModifiedBy = deletedBy;

                using (ReportDB reportDB = new ReportDB())
                {
                    operationDetails = reportDB.DeleteReport(reportModel); 
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

        public List<ConnectionModel> GetConnectionsByProject(int projectId)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            List<ConnectionModel> connectionList = null;
            try
            {
                using (ReportDB reportDB = new ReportDB())
                {
                    connectionList = reportDB.GetConnectionsByProject(projectId); 
                }
                return connectionList;
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

        public DataTable GetReportData(ConnectionModel connection, string storedProcedureName)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            DataTable dataTable = null;
            try
            {
                string connectionString = string.Format("Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = {0})(PORT = {1}))"
                + "(CONNECT_DATA =(SERVER = DEDICATED)(SID = {2})));User Id= {3};Password= {4};Persist Security Info=True;",
                connection.IpAddress,
                connection.PortNumber,
                connection.SID,
                connection.ConnectionUsername,
                connection.ConnectionPassword);

                using (ReportDB reportDB = new ReportDB(connectionString))
                {
                    dataTable = reportDB.GetReportData(storedProcedureName);
                }
                return dataTable;
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
