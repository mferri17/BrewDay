using System.Web;
using System.Web.Optimization;

namespace BrewDay
{
    public class BundleConfig
    {
        // Per altre informazioni sulla creazione di bundle, vedere https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/_wwwroot/vendor/jquery-ui/jquery-ui.css",
                "~/_wwwroot/vendor/bootstrap/css/bootstrap.css",
                "~/_wwwroot/vendor/datatables/datatables.css",
                "~/_wwwroot/css/site.css"));


            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/_wwwroot/vendor/jquery/jquery-3.2.1.js",
                "~/_wwwroot/vendor/jquery-ui/jquery-ui.js",
                "~/_wwwroot/vendor/bootstrap/js/bootstrap.js",
                "~/_wwwroot/vendor/respond/respond.js",
                "~/_wwwroot/vendor/datatables/datatables.js",
                "~/_wwwroot/js/site.js"));

        }
    }
}
