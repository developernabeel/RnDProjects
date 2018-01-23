using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SBIReportUtility.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Login",
                url: "login",
                defaults: new { controller = "Account", action = "Login" }
            );

            routes.MapRoute(
                name: "Logout",
                url: "logout",
                defaults: new { controller = "Account", action = "Logout" }
            );

            routes.MapRoute(
                name: "Dashboard",
                url: "dashboard",
                defaults: new { controller = "Project", action = "Dashboard" }
            );

            routes.MapRoute(
                name: "ProjectDetails",
                url: "project/{projectId}",
                defaults: new { controller = "Project", action = "Details" },
                constraints: new { projectId = @"\d+" }
            );

            routes.MapRoute(
                name: "ProjectUsers",
                url: "project/{projectId}/users",
                defaults: new { controller = "Project", action = "ProjectUsers" },
                constraints: new { projectId = @"\d+" }
            );

            routes.MapRoute(
                name: "ProjectConnections",
                url: "project/{projectId}/connections",
                defaults: new { controller = "Project", action = "ProjectConnections" },
                constraints: new { projectId = @"\d+" }
            );

            routes.MapRoute(
                name: "ProjectReports",
                url: "project/{projectId}/reports",
                defaults: new { controller = "Project", action = "ProjectReports" },
                constraints: new { projectId = @"\d+" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}