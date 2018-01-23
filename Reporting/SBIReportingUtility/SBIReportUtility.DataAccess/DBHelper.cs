using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Oracle.DataAccess.Client;
using SBIReportUtility.Entities;
using SBIReportUtility.Common.General;

namespace SBIReportUtility.DataAccess
{
    /// <summary>
    /// Class DBHelper.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class DBHelper : IDisposable
    {
        /// <summary>
        /// The log
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// The error log
        /// </summary>
        private static readonly ILog errorLog = LogManager.GetLogger("Error");

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string connectionString { get; set; }
        /// <summary>
        /// Gets or sets the oracle connection.
        /// </summary>
        /// <value>The oracle connection.</value>
        public OracleConnection oracleConnection { get; set; }
        /// <summary>
        /// Gets or sets the oracle command.
        /// </summary>
        /// <value>The oracle command.</value>
        public OracleCommand oracleCommand { get; set; }
        /// <summary>
        /// Gets or sets the oracle data reader.
        /// </summary>
        /// <value>The oracle data reader.</value>
        public OracleDataReader oracleDataReader { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="DBHelper"/> class.
        /// </summary>
        public DBHelper()
        {
            connectionString = ConfigurationManager.ConnectionStrings["SBIReportsConnection"].ConnectionString;
            oracleConnection = dbConection;
            oracleCommand = null;
            oracleDataReader = null;
        }

        public DBHelper(string connectionString)
        {
            this.connectionString = connectionString;
            oracleConnection = dbConection;
            oracleCommand = null;
            oracleDataReader = null;
        }


        /// <summary>
        /// The database sbiskm
        /// </summary>
        private OracleConnection _dbSBISKM;
        /// <summary>
        /// Gets the database conection.
        /// </summary>
        /// <value>The database conection.</value>
        public OracleConnection dbConection
        {
            get
            {
                if (_dbSBISKM == null)
                {
                    _dbSBISKM = new OracleConnection(connectionString);
                }
                return _dbSBISKM;
            }
        }

        /// <summary>
        /// Opens the connection.
        /// </summary>
        public void OpenConnection()
        {
            this.oracleConnection = oracleConnection;
            if (oracleConnection.State != ConnectionState.Open)
                oracleConnection.Open();
        }

        /// <summary>
        /// The disposed
        /// </summary>
        bool disposed = false;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DBHelper"/> class.
        /// </summary>
        ~DBHelper()
        {
            Dispose(false);
        }

        // Protected implementation of Dispose pattern. 
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                if (disposing)
                {
                    this.Dispose();
                    DisposeManagedResources();
                }

                DisposeUnmanagedResources();
                disposing = true;
            }
        }

        /// <summary>
        /// Disposes the managed resources.
        /// </summary>
        protected virtual void DisposeManagedResources()
        {

        }
        /// <summary>
        /// Disposes the unmanaged resources.
        /// </summary>
        protected virtual void DisposeUnmanagedResources() { }

        /// <summary>
        /// Inserts the update record.
        /// </summary>
        /// <param name="ObjParams">The object parameters.</param>
        /// <param name="StoredProcedureName">Name of the stored procedure.</param>
        /// <returns>OperationDetails.</returns>
        public OperationDetails InsertUpdateRecord(OracleParameter[] ObjParams, string StoredProcedureName)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            OperationDetails operationDetails = new OperationDetails();
            try
            {
                //oracleConnection = new OracleConnection(connectionString);
                oracleConnection.Open();

                oracleCommand = new OracleCommand();
                oracleCommand.Connection = oracleConnection;
                oracleCommand.CommandText = StoredProcedureName;
                oracleCommand.CommandType = CommandType.StoredProcedure;

                foreach (OracleParameter item in ObjParams)
                    oracleCommand.Parameters.Add(item);

                OracleParameter OperationStatus = new OracleParameter("OperationStatus", OracleDbType.Int32);
                OperationStatus.Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add(OperationStatus);
                OracleParameter OperationLogId = new OracleParameter("OperationLogId", OracleDbType.Int32);
                OperationLogId.Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add(OperationLogId);
                OracleParameter OperationMessage = new OracleParameter("OperationMessage", OracleDbType.Varchar2, 1000);
                OperationMessage.Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add(OperationMessage);

                //if (oracleConnection.State.ToString() == "Closed" )
                //{
                //    oracleConnection = new OracleConnection(connectionString);
                //    oracleConnection.Open();
                //    oracleCommand.Connection = oracleConnection;
                //}
                oracleDataReader = oracleCommand.ExecuteReader();

                operationDetails.OperationStatus = OperationStatus.Value.ToString() != DBNull.Value.ToString() ? Convert.ToInt32(OperationStatus.Value.ToString()) : 0;
                operationDetails.OperationLogId = OperationLogId.Value.ToString() != DBNull.Value.ToString() ? Convert.ToInt32(OperationLogId.Value.ToString()) : 0;
                operationDetails.OperationMessage = OperationMessage.Value.ToString();
            }
            catch (Exception exception)
            {
                errorLog.Fatal("Exception " + exception.Message + "\n" + exception.StackTrace);
                throw exception;
            }
            finally
            {
                if (oracleDataReader != null)
                {
                    oracleDataReader.Close();
                }
                if (oracleConnection != null)
                {
                    oracleConnection.Close();
                }
                log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution end.");
            }
            return operationDetails;
        }

        /// <summary>
        /// Insert/Update Data for Transfer Request - XMLType
        /// </summary>
        /// <param name="ObjParams">The object parameters.</param>
        /// <param name="XMLString">The XML string.</param>
        /// <param name="StoredProcedureName">Name of the stored procedure.</param>
        /// <returns>OperationDetails.</returns>
        public OperationDetails InsertUpdateXMLRecords(OracleParameter[] ObjParams, string XMLString, string StoredProcedureName)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            OperationDetails operationDetails = new OperationDetails();
            try
            {
                //oracleConnection = new OracleConnection(connectionString);
                oracleConnection.Open();

                oracleCommand = new OracleCommand();
                oracleCommand.Connection = oracleConnection;
                oracleCommand.CommandText = StoredProcedureName;
                oracleCommand.CommandType = CommandType.StoredProcedure;

                foreach (OracleParameter item in ObjParams)
                    oracleCommand.Parameters.Add(item);
                oracleCommand.Parameters.Add("DocumentDetails", OracleDbType.XmlType, XMLString, ParameterDirection.Input);

                OracleParameter OperationStatus = new OracleParameter("OperationStatus", OracleDbType.Int32);
                OperationStatus.Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add(OperationStatus);
                OracleParameter OperationLogId = new OracleParameter("OperationLogId", OracleDbType.Int32);
                OperationLogId.Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add(OperationLogId);
                OracleParameter OperationMessage = new OracleParameter("OperationMessage", OracleDbType.Varchar2, 1000);
                OperationMessage.Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add(OperationMessage);

                oracleDataReader = oracleCommand.ExecuteReader();

                operationDetails.OperationStatus = OperationStatus.Value.ToString() != DBNull.Value.ToString() ? Convert.ToInt32(OperationStatus.Value.ToString()) : 0;
                operationDetails.OperationLogId = OperationLogId.Value.ToString() != DBNull.Value.ToString() ? Convert.ToInt32(OperationLogId.Value.ToString()) : 0;
                operationDetails.OperationMessage = OperationMessage.Value.ToString();
            }
            catch (Exception exception)
            {
                errorLog.Fatal("Exception " + exception.Message + "\n" + exception.StackTrace);
                throw exception;
            }
            finally
            {
                if (oracleDataReader != null)
                {
                    oracleDataReader.Close();
                }
                if (oracleConnection != null)
                {
                    oracleConnection.Close();
                }
                log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution end.");
            }
            return operationDetails;
        }

        /// <summary>
        /// Reads the record.
        /// </summary>
        /// <param name="ObjParams">The object parameters.</param>
        /// <param name="StoredProcedureName">Name of the stored procedure.</param>
        /// <returns>DataTable.</returns>
        public DataTable ReadRecord(OracleParameter[] ObjParams, string StoredProcedureName)
        {
            DataTable dtRecords = new DataTable();

            try
            {
                //oracleConnection = new OracleConnection(connectionString);
                oracleConnection.Open();

                oracleCommand = new OracleCommand();
                oracleCommand.Connection = oracleConnection;
                oracleCommand.CommandText = StoredProcedureName;
                oracleCommand.CommandType = CommandType.StoredProcedure;

                foreach (OracleParameter item in ObjParams)
                    oracleCommand.Parameters.Add(item);

                oracleCommand.Parameters.Add("DBCursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                var da = new OracleDataAdapter(oracleCommand);
                da.Fill(dtRecords);

            }
            catch (Exception exception)
            {
                errorLog.Fatal("Exception " + exception.Message + "\n" + exception.StackTrace);
                throw exception;
            }
            finally
            {
                if (oracleDataReader != null)
                {
                    oracleDataReader.Close();
                }
                if (oracleConnection != null)
                {
                    oracleConnection.Close();
                }
                log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution end.");
            }
            return dtRecords;
        }

        /// <summary>
        /// Reads the record with paging sorting.
        /// </summary>
        /// <param name="ObjParams">The object parameters.</param>
        /// <param name="StoredProcedureName">Name of the stored procedure.</param>
        /// <param name="totalCount">The total count.</param>
        /// <returns>DataTable.</returns>
        public DataTable ReadRecordWithPagingSorting(OracleParameter[] ObjParams, string StoredProcedureName, out int totalCount)
        {
            DataTable dtRecords = new DataTable();

            try
            {
                //oracleConnection = new OracleConnection(connectionString);
                oracleConnection.Open();

                oracleCommand = new OracleCommand();
                oracleCommand.Connection = oracleConnection;
                oracleCommand.CommandText = StoredProcedureName;
                oracleCommand.CommandType = CommandType.StoredProcedure;
                //oracleCommand.BindByName = true;

                foreach (OracleParameter item in ObjParams)
                    oracleCommand.Parameters.Add(item);

                OracleParameter TotalCount = new OracleParameter("TotalCount", OracleDbType.Int32);
                TotalCount.Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add(TotalCount);

                oracleCommand.Parameters.Add("DBCursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                var da = new OracleDataAdapter(oracleCommand);
                da.Fill(dtRecords);

                totalCount = TotalCount.Value.ToString() != DBNull.Value.ToString() ? Convert.ToInt32(TotalCount.Value.ToString()) : 0;

            }
            catch (Exception exception)
            {
                errorLog.Fatal("Exception " + exception.Message + "\n" + exception.StackTrace);
                throw exception;
            }
            finally
            {
                if (oracleDataReader != null)
                {
                    oracleDataReader.Close();
                }
                if (oracleConnection != null)
                {
                    oracleConnection.Close();
                }
                log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution end.");
            }
            return dtRecords;
        }

        /// <summary>
        /// Reads the record with four out parameters.
        /// </summary>
        /// <param name="ObjParams">The object parameters.</param>
        /// <param name="StoredProcedureName">Name of the stored procedure.</param>
        /// <param name="val1">The val1.</param>
        /// <param name="val2">The val2.</param>
        /// <param name="val3">The val3.</param>
        /// <param name="val4">The val4.</param>
        /// <returns>DataTable.</returns>
        public DataTable ReadRecordWithFourOutParameters(OracleParameter[] ObjParams, string StoredProcedureName, out int val1, out int val2, out int val3, out int val4)
        {
            DataTable dtRecords = new DataTable();

            try
            {
                //oracleConnection = new OracleConnection(connectionString);
                oracleConnection.Open();

                oracleCommand = new OracleCommand();
                oracleCommand.Connection = oracleConnection;
                oracleCommand.CommandText = StoredProcedureName;
                oracleCommand.CommandType = CommandType.StoredProcedure;

                foreach (OracleParameter item in ObjParams)
                    oracleCommand.Parameters.Add(item);

                OracleParameter Value1 = new OracleParameter("TotalCount", OracleDbType.Int32);
                Value1.Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add(Value1);

                OracleParameter Value2 = new OracleParameter("Value1", OracleDbType.Int32);
                Value2.Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add(Value2);

                OracleParameter Value3 = new OracleParameter("Value2", OracleDbType.Int32);
                Value3.Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add(Value3);

                OracleParameter Value4 = new OracleParameter("Value4", OracleDbType.Int32);
                Value4.Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add(Value4);

                oracleCommand.Parameters.Add("DBCursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                oracleCommand.ExecuteNonQuery();

                var da = new OracleDataAdapter(oracleCommand);
                da.Fill(dtRecords);

                val1 = Value1.Value.ToString() != DBNull.Value.ToString() ? Convert.ToInt32(Value1.Value.ToString()) : 0;
                val2 = Value2.Value.ToString() != DBNull.Value.ToString() ? Convert.ToInt32(Value2.Value.ToString()) : 0;
                val3 = Value3.Value.ToString() != DBNull.Value.ToString() ? Convert.ToInt32(Value3.Value.ToString()) : 0;
                val4 = Value4.Value.ToString() != DBNull.Value.ToString() ? Convert.ToInt32(Value4.Value.ToString()) : 0;

            }
            catch (Exception exception)
            {
                errorLog.Fatal("Exception " + exception.Message + "\n" + exception.StackTrace);
                throw exception;
            }
            finally
            {
                if (oracleDataReader != null)
                {
                    oracleDataReader.Close();
                }
                if (oracleConnection != null)
                {
                    oracleConnection.Close();
                }
                log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution end.");
            }
            return dtRecords;
        }

        /// <summary>
        /// Reads the record with one out parameters.
        /// </summary>
        /// <param name="ObjParams">The object parameters.</param>
        /// <param name="StoredProcedureName">Name of the stored procedure.</param>
        /// <param name="value1">The value1.</param>
        /// <returns>DataTable.</returns>
        public DataTable ReadRecordWithOneOutParameters(OracleParameter[] ObjParams, string StoredProcedureName, out int value1)
        {
            DataTable dtRecords = new DataTable();

            try
            {
                //oracleConnection = new OracleConnection(connectionString);
                oracleConnection.Open();

                oracleCommand = new OracleCommand();
                oracleCommand.Connection = oracleConnection;
                oracleCommand.CommandText = StoredProcedureName;
                oracleCommand.CommandType = CommandType.StoredProcedure;

                foreach (OracleParameter item in ObjParams)
                    oracleCommand.Parameters.Add(item);

                OracleParameter Value1 = new OracleParameter("Value1", OracleDbType.Int32);
                Value1.Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add(Value1);

                oracleCommand.Parameters.Add("DBCursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                oracleCommand.ExecuteNonQuery();

                var da = new OracleDataAdapter(oracleCommand);
                da.Fill(dtRecords);

                value1 = Value1.Value.ToString() != DBNull.Value.ToString() ? Convert.ToInt32(Value1.Value.ToString()) : 0;

            }
            catch (Exception exception)
            {
                errorLog.Fatal("Exception " + exception.Message + "\n" + exception.StackTrace);
                throw exception;
            }
            finally
            {
                if (oracleDataReader != null)
                {
                    oracleDataReader.Close();
                }
                if (oracleConnection != null)
                {
                    oracleConnection.Close();
                }
                log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution end.");
            }
            return dtRecords;
        }

        /// <summary>
        /// Reads the record with two int out parameters.
        /// </summary>
        /// <param name="ObjParams">The object parameters.</param>
        /// <param name="StoredProcedureName">Name of the stored procedure.</param>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns>DataTable.</returns>
        public DataTable ReadRecordWithTwoIntOutParameters(OracleParameter[] ObjParams, string StoredProcedureName, out int value1, out int value2)
        {
            DataTable dtRecords = new DataTable();

            try
            {
                //oracleConnection = new OracleConnection(connectionString);
                oracleConnection.Open();

                oracleCommand = new OracleCommand();
                oracleCommand.Connection = oracleConnection;
                oracleCommand.CommandText = StoredProcedureName;
                oracleCommand.CommandType = CommandType.StoredProcedure;

                foreach (OracleParameter item in ObjParams)
                    oracleCommand.Parameters.Add(item);

                OracleParameter Value1 = new OracleParameter("Value1", OracleDbType.Int32);
                Value1.Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add(Value1);

                OracleParameter Value2 = new OracleParameter("Value2", OracleDbType.Int32);
                Value2.Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add(Value2);

                oracleCommand.Parameters.Add("DBCursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                oracleCommand.ExecuteNonQuery();

                var da = new OracleDataAdapter(oracleCommand);
                da.Fill(dtRecords);

                value1 = Value1.Value.ToString() != DBNull.Value.ToString() ? Convert.ToInt32(Value1.Value.ToString()) : 0;
                value2 = Value2.Value.ToString() != DBNull.Value.ToString() ? Convert.ToInt32(Value2.Value.ToString()) : 0;

            }
            catch (Exception exception)
            {
                errorLog.Fatal("Exception " + exception.Message + "\n" + exception.StackTrace);
                throw exception;
            }
            finally
            {
                if (oracleDataReader != null)
                {
                    oracleDataReader.Close();
                }
                if (oracleConnection != null)
                {
                    oracleConnection.Close();
                }
                log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution end.");
            }
            return dtRecords;
        }

        /// <summary>
        /// Reads the record with two out parameters.
        /// </summary>
        /// <param name="ObjParams">The object parameters.</param>
        /// <param name="StoredProcedureName">Name of the stored procedure.</param>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns>DataTable.</returns>
        public DataTable ReadRecordWithTwoOutParameters(OracleParameter[] ObjParams, string StoredProcedureName, out string value1, out string value2)
        {
            DataTable dtRecords = new DataTable();

            try
            {
                //oracleConnection = new OracleConnection(connectionString);
                oracleConnection.Open();

                oracleCommand = new OracleCommand();
                oracleCommand.Connection = oracleConnection;
                oracleCommand.CommandText = StoredProcedureName;
                oracleCommand.CommandType = CommandType.StoredProcedure;

                foreach (OracleParameter item in ObjParams)
                    oracleCommand.Parameters.Add(item);

                OracleParameter Value1 = new OracleParameter("Value1", OracleDbType.Varchar2);
                Value1.Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add(Value1);

                OracleParameter Value2 = new OracleParameter("Value2", OracleDbType.Varchar2);
                Value2.Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add(Value2);

                oracleCommand.Parameters.Add("DBCursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                oracleCommand.ExecuteNonQuery();

                var da = new OracleDataAdapter(oracleCommand);
                da.Fill(dtRecords);

                value1 = Value1.Value.ToString() != DBNull.Value.ToString() ? Value1.Value.ToString() : "";
                value2 = Value2.Value.ToString() != DBNull.Value.ToString() ? Value2.Value.ToString() : "";

            }
            catch (Exception exception)
            {
                errorLog.Fatal("Exception " + exception.Message + "\n" + exception.StackTrace);
                throw exception;
            }
            finally
            {
                if (oracleDataReader != null)
                {
                    oracleDataReader.Close();
                }
                if (oracleConnection != null)
                {
                    oracleConnection.Close();
                }
                log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution end.");
            }
            return dtRecords;
        }

        /// <summary>
        /// Reads the records with XML data.
        /// </summary>
        /// <param name="ObjParams">The object parameters.</param>
        /// <param name="XMLString">The XML string.</param>
        /// <param name="StoredProcedureName">Name of the stored procedure.</param>
        /// <returns>DataTable.</returns>
        public DataTable ReadRecordsWithXMLData(OracleParameter[] ObjParams, string XMLString, string StoredProcedureName)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            DataTable dtRecords = new DataTable();
            try
            {
                //oracleConnection = new OracleConnection(connectionString);
                oracleConnection.Open();

                oracleCommand = new OracleCommand();
                oracleCommand.Connection = oracleConnection;
                oracleCommand.CommandText = StoredProcedureName;
                oracleCommand.CommandType = CommandType.StoredProcedure;

                foreach (OracleParameter item in ObjParams)
                    oracleCommand.Parameters.Add(item);
                oracleCommand.Parameters.Add("DocumentDetails", OracleDbType.XmlType, XMLString, ParameterDirection.Input);
                oracleCommand.Parameters.Add("DBCursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                var da = new OracleDataAdapter(oracleCommand);
                da.Fill(dtRecords);

            }
            catch (Exception exception)
            {
                errorLog.Fatal("Exception " + exception.Message + "\n" + exception.StackTrace);
                throw exception;
            }
            finally
            {
                if (oracleDataReader != null)
                {
                    oracleDataReader.Close();
                }
                if (oracleConnection != null)
                {
                    oracleConnection.Close();
                }
                log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution end.");
            }
            return dtRecords;
        }

        /// <summary>
        /// Reads the records with XML data with one out parameter.
        /// </summary>
        /// <param name="ObjParams">The object parameters.</param>
        /// <param name="XMLString">The XML string.</param>
        /// <param name="StoredProcedureName">Name of the stored procedure.</param>
        /// <param name="value">The value.</param>
        /// <returns>DataTable.</returns>
        public DataTable ReadRecordsWithXMLDataWithOneOutParameter(OracleParameter[] ObjParams, string XMLString, string StoredProcedureName, out int value)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            DataTable dtRecords = new DataTable();
            try
            {
                //oracleConnection = new OracleConnection(connectionString);
                oracleConnection.Open();

                oracleCommand = new OracleCommand();
                oracleCommand.Connection = oracleConnection;
                oracleCommand.CommandText = StoredProcedureName;
                oracleCommand.CommandType = CommandType.StoredProcedure;

                foreach (OracleParameter item in ObjParams)
                    oracleCommand.Parameters.Add(item);

                OracleParameter Value1 = new OracleParameter("Value1", OracleDbType.Int32);
                Value1.Direction = ParameterDirection.Output;
                oracleCommand.Parameters.Add(Value1);

                oracleCommand.Parameters.Add("DocumentDetails", OracleDbType.XmlType, XMLString, ParameterDirection.Input);
                oracleCommand.Parameters.Add("DBCursor", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                var da = new OracleDataAdapter(oracleCommand);
                da.Fill(dtRecords);

                value = Value1.Value.ToString() != DBNull.Value.ToString() ? Convert.ToInt32(Value1.Value.ToString()) : 0;
            }
            catch (Exception exception)
            {
                errorLog.Fatal("Exception " + exception.Message + "\n" + exception.StackTrace);
                throw exception;
            }
            finally
            {
                if (oracleDataReader != null)
                {
                    oracleDataReader.Close();
                }
                if (oracleConnection != null)
                {
                    oracleConnection.Close();
                }
                log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution end.");
            }
            return dtRecords;
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cmdType">Type of the command.</param>
        /// <returns>System.Int32.</returns>
        public int ExecuteCommand(string command, int cmdType = 1)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            try
            {
                int result = 0;
                oracleConnection = new OracleConnection(connectionString);
                oracleConnection.Open();

                oracleCommand = new OracleCommand();
                oracleCommand.Connection = oracleConnection;
                oracleCommand.CommandText = command;
                if (cmdType == 1)
                    result = oracleCommand.ExecuteNonQuery();
                else
                    result = Convert.ToInt32(oracleCommand.ExecuteScalar());
                CommitCommand();
                return result;
            }
            catch (Exception exception)
            {
                errorLog.Fatal("Exception " + exception.Message + "\n" + exception.StackTrace);
                throw exception;
            }
            finally
            {
                if (oracleDataReader != null)
                {
                    oracleDataReader.Close();
                }
                if (oracleConnection != null)
                {
                    oracleConnection.Close();
                }
                log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution end.");
            }
        }

        /// <summary>
        /// Commits the command.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int CommitCommand()
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            try
            {
                //oracleConnection = new OracleConnection(connectionString);
                //oracleConnection.Open();

                oracleCommand = new OracleCommand();
                oracleCommand.Connection = oracleConnection;
                oracleCommand.CommandText = "commit";
                int result = oracleCommand.ExecuteNonQuery();
                return result;
            }
            catch (Exception exception)
            {
                errorLog.Fatal("Exception " + exception.Message + "\n" + exception.StackTrace);
                throw exception;
            }
            finally
            {
                if (oracleDataReader != null)
                {
                    oracleDataReader.Close();
                }
                if (oracleConnection != null)
                {
                    oracleConnection.Close();
                }
                log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution end.");
            }
        }
    }
}
