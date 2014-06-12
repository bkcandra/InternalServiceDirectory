using System.Web;
using System.Web.Optimization;

namespace ISD.Application.provider.MVC
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/unifi").Include(
                //JS Global Compulsory
                             "~/assets/plugins/jquery-1.10.2.min.js",
                             "~/assets/plugins/jquery-migrate-1.2.1.min.js",
                "~/assets/plugins/bootstrap/js/bootstrap.min.js",

                //JS Implementing Plugins 
                             "~/assets/plugins/back-to-top.js",
                             "~/assets/plugins/owl-carousel/owl-carousel/owl.carousel.js",
                             "~/assets/plugins/revolution_slider/rs-plugin/js/jquery.themepunch.revolution.min.js",
                //JS Page Level 
                             "~/assets/js/app.js",
                             "~/assets/js/pages/index.js",
                             "~/assets/js/plugins/owl-carousel.js"));




            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
