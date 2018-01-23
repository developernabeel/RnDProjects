using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBIReportUtility.Common.General
{
    public class EnumHelper
    {
        public enum Role
        {
            SuperAdmin = 1,
            User = 2
        }

        public enum SubRole
        {
            ProjectOwner = 1,
            ReportUser = 2
        }

        public enum HrmsServiceType
        {
            Production = 1,
            Quality = 2
        }
    }
}
