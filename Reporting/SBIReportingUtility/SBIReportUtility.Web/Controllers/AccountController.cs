using log4net;
using SBIReportUtility.BusinessLayer.Implementation;
using SBIReportUtility.Common.General;
using SBIReportUtility.Entities;
using SBIReportUtility.Web.Models.Account;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SBIReportUtility.Web.Controllers
{
    public class AccountController : BaseController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly ILog errorLog = LogManager.GetLogger("Error");

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            log.Debug(MethodHelper.GetCurrentMethodName() + " Method execution start.");
            try
            {
                if (ModelState.IsValid)
                {
                    AccountBL accountBL = new AccountBL();
                    
                    string useLoginService = System.Configuration.ConfigurationManager.AppSettings["UseLoginService"];
                    if (useLoginService == "1")
                    {
                        var isValid = accountBL.ValidateUserFromService(loginViewModel.Pfid, loginViewModel.Password);
                        if (!isValid)
                        {
                            TempData["ErrorMessage"] = "Incorrect username or password.";
                            return View(loginViewModel);
                        }
                    }

                    UserModel userModel = accountBL.GetUserRole(loginViewModel.Pfid);
                    if (userModel != null)
                    {
                        FormsAuthentication.SetAuthCookie(loginViewModel.Pfid.ToString(), false);
                        Session["CurrentUser"] = userModel;
                        if (userModel.RoleId == (int)EnumHelper.Role.SuperAdmin)
                            return RedirectToAction("Index", "Home");
                        return RedirectToAction("Dashboard", "Project");
                    }
                    TempData["ErrorMessage"] = "Incorrect username or password.";
                }
                return View(loginViewModel);
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

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            Response.Cookies.Add(cookie);
            HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Response.Cache.SetNoServerCaching();
            HttpContext.Response.Cache.SetNoStore();

            // Clears session cookie so that a new session id is generated when the user logins.
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));

            return RedirectToAction("Login", "Account");
        }

        [ChildActionOnly]
        public ActionResult SideMenu()
        {
            return PartialView("~/Views/Shared/_SideMenu.cshtml", CurrentUser);
        }

        [ChildActionOnly]
        public ActionResult Header()
        {
            return PartialView("~/Views/Shared/_Header.cshtml", CurrentUser);
        }
    }
}
