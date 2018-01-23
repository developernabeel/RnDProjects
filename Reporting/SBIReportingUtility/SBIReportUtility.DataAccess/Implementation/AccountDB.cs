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
    public class AccountDB : DBHelper
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ILog errorLog = LogManager.GetLogger("Error");
        
        public UserRoleModel GetUserRole(int Pfid)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            UserRoleModel userRoleModel = null;
            try
            {
                OracleParameter[] objParams = { 
                                                new OracleParameter("P_PFID", Pfid)
                                              };

                DataTable dtRecords = new DataTable();
                dtRecords = ReadRecord(objParams, "GET_USER_ROLE");

                if (dtRecords.Rows.Count > 0)
                {
                    userRoleModel = new UserRoleModel();
                    userRoleModel.Id = dtRecords.Rows[0]["ID"] == DBNull.Value ? 0 : Convert.ToInt32(dtRecords.Rows[0]["ID"].ToString());
                    userRoleModel.Pfid = dtRecords.Rows[0]["PFINDEXNO"] == DBNull.Value ? 0 : Convert.ToInt32(dtRecords.Rows[0]["PFINDEXNO"].ToString());
                    userRoleModel.Name = dtRecords.Rows[0]["NAME"] == DBNull.Value ? "" : dtRecords.Rows[0]["NAME"].ToString();
                    userRoleModel.RoleId = dtRecords.Rows[0]["ROLEID"] == DBNull.Value ? 0 : Convert.ToInt32(dtRecords.Rows[0]["ROLEID"].ToString());
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
            return userRoleModel;
        }
    }
}
