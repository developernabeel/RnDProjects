using SBIReportUtility.BusinessLayer.HRMSQuality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBIReportUtility.BusinessLayer.HrmsService
{
    public interface IHrmsService
    {
        string ValidateUser(string userName, string password);
        ArrayOfString GetAuthentication(string userName, string password);
    }
}
