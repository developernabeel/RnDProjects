using System.Web;
using System.Web.Optimization;

namespace SBIReportUtility.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                        "~/Content/bootstrap.min.css",
                        "~/Content/metisMenu.min.css",
                        "~/Scripts/datatables-plugins/dataTables.bootstrap.css",
                        "~/Scripts/datatables-responsive/dataTables.responsive.css",
                        "~/Content/sb-admin-2.min.css",
                        "~/Content/style.css"
                        ).Include("~/Content/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransform()));

            bundles.Add(new ScriptBundle("~/bundles/appscripts").Include(
                        "~/Scripts/jquery-{version}.min.js",
                        "~/Scripts/bootstrap.min.js",
                        "~/Scripts/metisMenu.min.js",
                        "~/Scripts/sb-admin-2.min.js",
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatable").Include(
                        "~/Scripts/datatables/js/jquery.dataTables.min.js",
                        "~/Scripts/datatables-plugins/dataTables.bootstrap.min.js",
                        "~/Scripts/datatables-responsive/dataTables.responsive.js"));

            BundleTable.EnableOptimizations = true;
        }
    }
}