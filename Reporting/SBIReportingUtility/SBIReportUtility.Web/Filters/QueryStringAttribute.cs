using SBIReportUtility.Common.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBIReportUtility.Web.Filters
{
    public class QueryStringAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!string.IsNullOrEmpty(filterContext.HttpContext.Request.QueryString["id"]))
            {
                if (string.IsNullOrEmpty(filterContext.HttpContext.Request.QueryString["hash"]))
                {
                    filterContext.Result = new RedirectResult("~/Account/Unauthorize");
                }
            }
            if (!string.IsNullOrEmpty(filterContext.HttpContext.Request.QueryString["id"]) && !string.IsNullOrEmpty(filterContext.HttpContext.Request.QueryString["hash"]))
            {
                if (!VerifyMD5HashValue(filterContext.HttpContext.Request.QueryString["id"], filterContext.HttpContext.Request.QueryString["hash"]))
                {
                    filterContext.Result = new RedirectResult("~/Account/Unauthorize");
                }
            }

        }

        public bool VerifyMD5HashValue(string id, string hash)
        {
            return EncryptionHelper.VerifyMD5Hash(id, hash);
        }
    }
}