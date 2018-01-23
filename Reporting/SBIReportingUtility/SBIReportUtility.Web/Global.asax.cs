using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using log4net;

namespace SBIReportUtility.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            log4net.Config.XmlConfigurator.Configure();

            MvcHandler.DisableMvcResponseHeader = true;
        }

        void Application_Error(object sender, EventArgs e)
        {
            bool IsAjax = new HttpRequestWrapper(System.Web.HttpContext.Current.Request).IsAjaxRequest();
            Exception exception = Server.GetLastError();
            Response.Clear();            

            HttpException httpException = exception as HttpException;

            string action = "Error";

            if (httpException != null)
            {
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        // page not found
                        action = "HttpError404";
                        HttpContext.Current.Response.StatusCode = 404;
                        HttpContext.Current.Response.StatusDescription = "Not Found";
                        break;
                    default:
                        break;
                }
            }
            else
            {                
                HttpContext.Current.Response.StatusCode = 500;
                HttpContext.Current.Response.StatusDescription = "Error";
            }

            // clear error on server
            Server.ClearError();
            if (IsAjax) 
                HttpContext.Current.Response.SuppressFormsAuthenticationRedirect = true;
            else
                Response.Redirect(String.Format("~/Error/{0}/", action));
        }

        protected void Application_BeginRequest()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            HttpContext.Current.Response.AddHeader("x-frame-options", "DENY");
        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Headers.Remove("Server");
        }
    }
}