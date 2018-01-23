using SBIReportUtility.Common.General;
using SBIReportUtility.Entities;
using SBIReportUtility.Web.AuthenticationExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace SBIReportUtility.Web.Filters
{
    public class ReportAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public string Permissions { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var CurrentUser = HttpContext.Current.User;
            if (CurrentUser != null && CurrentUser.Identity != null && filterContext.HttpContext.Request.IsAuthenticated)
            {
                if (HttpContext.Current.Session["CurrentUser"] != null)
                {
                    Authentication auth = new Authentication();
                    auth.UserContext = (UserModel)HttpContext.Current.Session["CurrentUser"];

                    if (!string.IsNullOrEmpty(Permissions))
                    {
                        if (CurrentUser != null)
                        {
                            EnumHelper.Role userRole = (EnumHelper.Role)int.Parse(auth.UserContext.RoleId.ToString());
                            string[] permissionList = Permissions.Replace(" ", "").Split(',');
                            var isValidated = false;
                            foreach (var permission in permissionList)
                            {
                                if (permission == userRole.ToString())
                                {
                                    isValidated = true;
                                    break;
                                }
                            }
                            if (!isValidated)
                            {
                                filterContext.Controller.TempData["ErrorMessage"] = "Unauthorized Access.";
                                filterContext.Result = new RedirectResult("~/Account/Login");
                            }
                        }
                    }
                    auth.Identity = HttpContext.Current.User.Identity;
                    HttpContext.Current.User = auth;
                }
                else
                {
                    if (!filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        FormsAuthentication.SignOut();
                        filterContext.Controller.TempData["WarningMessage"] = "Your Login Expired";
                        filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary {
                                            { "Controller", "Account" },
                                            { "Action", "Login" }});
                    }
                    else
                    {
                        filterContext.Controller.TempData["WarningMessage"] = "Your Login Expired";
                        filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary {
                                            { "Controller", "Account" },
                                            { "Action", "Login" }});
                    }
                }
            }
            else
            {
                FormsAuthentication.SignOut();
                filterContext.Controller.TempData["WarningMessage"] = "Your Login Expired";

                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    HttpContext.Current.Response.StatusCode = 401;
                    HttpContext.Current.Response.StatusDescription = "Authentication required";
                    HttpContext.Current.Response.SuppressFormsAuthenticationRedirect = true;

                    filterContext.Result = new JsonResult
                    {
                        Data = new
                        {
                            Code = "888",
                            message = "logOut"
                        },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary {
                    { "Controller", "Account" },
                    { "Action", "Login" }});
                }
            }
        }
    }
}