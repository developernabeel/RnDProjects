using log4net;
using SBIReportUtility.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBIReportUtility.Web.Controllers
{
    public class BaseController : Controller
    {
        public UserModel CurrentUser
        {
            get
            {
                if ((User as SBIReportUtility.Web.AuthenticationExt.Authentication) != null)
                    return (User as SBIReportUtility.Web.AuthenticationExt.Authentication).UserContext;
                return null;
            }
        }
    }
}
