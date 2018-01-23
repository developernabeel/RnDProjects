using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SBIReportUtility.Common.General;
using System.Configuration;
using SBIReportUtility.Entities;
using log4net;
using SBIReportUtility.BusinessLayer.HRMSQuality;

namespace SBIReportUtility.BusinessLayer.HrmsService
{
    public class SapAccess
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ILog errorLog = LogManager.GetLogger("Error");
        private readonly IHrmsService _hrmsService;

        public SapAccess()
        {
            string serviceTypeConfig = ConfigurationManager.AppSettings["HrmsServiceType"];
            EnumHelper.HrmsServiceType serviceType = (EnumHelper.HrmsServiceType)Convert.ToInt32(serviceTypeConfig);

            if (serviceType == EnumHelper.HrmsServiceType.Production)
                _hrmsService = new HrmsProduction();

            else if (serviceType == EnumHelper.HrmsServiceType.Quality)
                _hrmsService = new HrmsQuality();
        }

        public SapAccess(IHrmsService hrmsService)
        {
            _hrmsService = hrmsService;
        }

        public bool ValidateUser(string userName, string password)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            try
            {
                return _hrmsService.ValidateUser(userName, password) == "User Verified";                
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

        public UserModel GetUserDetails(string userName)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            try
            {
                ArrayOfString data = _hrmsService.GetAuthentication(userName, "a");

                if (data == null || data.Count == 0 || (string.IsNullOrEmpty(data[4]) && string.IsNullOrEmpty(data[5])))
                    return null;

                UserModel userModel = new UserModel();
                userModel.Pfid = Convert.ToInt32(userName);
                userModel.Name = data[4] + " " + data[5];
                userModel.EmailId = data[7];
                userModel.Designation = data[6];
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
    }
}
