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
    public class ConnectionBL
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ILog errorLog = LogManager.GetLogger("Error");
        
        /// <summary>
        /// Gets all the connections which are not deleted.
        /// </summary>
        /// <returns>List of ConnectionModel</returns>
        public List<ConnectionModel> GetConnectionList()
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            List<ConnectionModel> connectionList = null;
            try
            {
                using (ConnectionDB connectionDB = new ConnectionDB())
                {
                    connectionList = connectionDB.GetConnectionList();
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
                using (ConnectionDB connectionDB = new ConnectionDB())
                {
                    operationDetails = connectionDB.DeleteConnection(connectionId, deletedBy);
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
        /// Get connection by Id.
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        public ConnectionModel GetConnectionById(int connectionId)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            ConnectionModel connectionModel = null;
            try
            {
                using (ConnectionDB connectionDB = new ConnectionDB())
                {
                    connectionModel = connectionDB.GetConnectionById(connectionId);
                    connectionModel.ConnectionPassword = EncryptionHelper.Decrypt(connectionModel.ConnectionPassword);
                    return connectionModel; 
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
                using (ConnectionDB connectionDB = new ConnectionDB())
                {
                    connectionModel.ConnectionPassword = EncryptionHelper.Encrypt(connectionModel.ConnectionPassword);
                    operationDetails = connectionDB.AddEditConnection(connectionModel);
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
        /// Runs a sample query to test if database connection is successful.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public int TestConnection(ConnectionModel connection)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            try
            {
                string connectionString = string.Format("Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = {0})(PORT = {1}))"
                + "(CONNECT_DATA =(SERVER = DEDICATED)(SID = {2})));User Id= {3};Password= {4};Persist Security Info=True;",
                connection.IpAddress, connection.PortNumber, connection.SID, connection.ConnectionUsername, connection.ConnectionPassword);

                using (ConnectionDB connectionDB = new ConnectionDB(connectionString))
                {
                    return connectionDB.TestConnection();
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
