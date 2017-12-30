using System.Web;
using System.Web.Optimization;

namespace BrewDay
{
    public class BundleConfig
    {
        // For other information aboud bundles creation, see https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            // CSS Bundle (rendered at top of the page)

            bundles.Add(new StyleBundle("~/bundles/vendor/css").Include(
                "~/_wwwroot/vendor/jquery-ui/jquery-ui.css",
                "~/_wwwroot/vendor/bootstrap/css/bootstrap.css",
                "~/_wwwroot/vendor/datatables/datatables.css"));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/_wwwroot/css/site.css"));


            // JS Bundle (rendered at bottom of the page)

            bundles.Add(new ScriptBundle("~/bundles/vendor/js").Include(
                "~/_wwwroot/vendor/jquery/jquery-3.2.1.js",
                "~/_wwwroot/vendor/jquery-ui/jquery-ui.js",
                "~/_wwwroot/vendor/jquery-ui/external/datepicker-it.js",
                "~/_wwwroot/vendor/bootstrap/js/bootstrap.js",
                "~/_wwwroot/vendor/respond/respond.js",
                "~/_wwwroot/vendor/datatables/datatables.js"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/_wwwroot/js/string-util.js",
                "~/_wwwroot/js/submit-helper.js",
                "~/_wwwroot/js/site.js"));

        }
    }
}
