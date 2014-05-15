using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.UI;

namespace ISD.Administration.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkID=303951
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/WebFormsJs").Include(
                            "~/Scripts/WebForms/WebForms.js",
                            "~/Scripts/WebForms/WebUIValidation.js",
                            "~/Scripts/WebForms/MenuStandards.js",
                            "~/Scripts/WebForms/Focus.js",
                            "~/Scripts/WebForms/GridView.js",
                            "~/Scripts/WebForms/DetailsView.js",
                            "~/Scripts/WebForms/TreeView.js",
                            "~/Scripts/WebForms/WebParts.js",
                            "~/Scripts/morris/morris.min.js",
                            "~/Scripts/sparkline/jquery.sparkline.min.js",
                            "~/Scripts/jvectormap/jquery-jvectormap-1.2.2.min.js",
                            "~/Scripts/jvectormap/jquery-jvectormap-world-mill-en.js",
                            "~/Scripts/fullcalendar/fullcalendar.min.js",
                            "~/Scripts/jqueryKnob/jquery.knob.js",
                            "~/Scripts/daterangepicker/daterangepicker.js",
                            "~/Scripts/bootstrap-wysihtml5/bootstrap3-wysihtml5.all.min.js",
                            "~/Scripts/iCheck/icheck.min.js",
                             "~/Scripts/AdminLTE/app.js"));

            // Order is very important for these files to work, they have explicit dependencies
            bundles.Add(new ScriptBundle("~/bundles/MsAjaxJs").Include(
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjax.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxApplicationServices.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxTimer.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxWebForms.js"));

            // Use the Development version of Modernizr to develop with and learn from. Then, when you’re
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                            "~/Scripts/modernizr-*"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;

            ScriptManager.ScriptResourceMapping.AddDefinition(
                "respond",
                new ScriptResourceDefinition
                {
                    Path = "~/Scripts/respond.min.js",
                    DebugPath = "~/Scripts/respond.js",
                });
        }
    }
}