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
    public class AccountBL
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ILog errorLog = LogManager.GetLogger("Error");

        public UserModel GetUserRole(int pfid)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            try
            {
                using (AccountDB accountDB = new AccountDB())
                {
                    UserModel userModel = null;
                    UserRoleModel userRoleModel = accountDB.GetUserRole(pfid);
                    if (userRoleModel != null)
                    {
                        userModel = new UserModel
                        {
                            Pfid = pfid,
                            Name = userRoleModel.Name,
                            RoleId = userRoleModel.RoleId
                        };
                    }
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
        /// Validates and gets user details by PFID and password from HRMS service.
        /// </summary>
        /// <param name="pfid">Employee PFID</param>
        /// <param name="password">Employee Password</param>
        /// <returns>True if valid user else false.</returns>
        public bool ValidateUserFromService(int pfid, string password)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            try
            {
                SapAccess sapAccess = new SapAccess();
                return sapAccess.ValidateUser(pfid.ToString(), password);
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
