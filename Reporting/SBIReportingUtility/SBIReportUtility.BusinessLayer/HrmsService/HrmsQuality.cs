using SBIReportUtility.BusinessLayer.HRMSQuality;

namespace SBIReportUtility.BusinessLayer.HrmsService
{
    internal class HrmsQuality : IHrmsService
    {
        private readonly ValidatencuserwebserviceViClient _dcvQuality;

        public HrmsQuality()
        {
            _dcvQuality = new ValidatencuserwebserviceViClient();
            System.Net.ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslerror) => true;            
        }

        public string ValidateUser(string userName, string password)
        {
            return _dcvQuality.Validateuser(userName, password);
        }

        public ArrayOfString GetAuthentication(string userName, string password)
        {
            return _dcvQuality.getAuthentication(userName, password);
        }
    }
}
