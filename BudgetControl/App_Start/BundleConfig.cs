﻿using System.Web;
using System.Web.Optimization;

namespace BudgetControl
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //BundleTable.EnableOptimizations = true;
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap-cerulean.css",
                      "~/Content/ngProgress.css",
                      "~/Content/Site.css",
                      "~/Content/Loader.css",
                      "~/Content/font-awesome.min.css",
                      "~/Scripts/angular-ui-grid/ui-grid.min.css",
                      "~/Scripts/DataTables/datatables.min.css",
                      "~/Content/tiny-date-picker.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/angular")
                .Include(
                    "~/Scripts/underscore-min.js",
                    "~/Scripts/angular.js",
                    "~/Scripts/angular-ui-router.min.js",
                    "~/Scripts/angular-resource.min.js",
                    "~/Scripts/angular-route.min.js",
                    "~/Scripts/angular-animate.min.js",
                    "~/Scripts/angular-touch.min.js",
                    "~/Scripts/ngprogress.min.js",
                    "~/Scripts/ui-bootstrap-tpls-2.5.0.min.js",
                    "~/Scripts/sweetalert211.js",
                    "~/Scripts/tiny-date-picker.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/polyfill")
                .Include(
                    "~/js/polyfill.js"
                    ));

            bundles.Add(new ScriptBundle("~/bundles/angular-script")
                  .Include(
                      "~/js/app.js",
                      "~/js/app.run.js",
                      "~/js/app.route.js",
                      "~/js/app.filter.js",
                      "~/js/handle.response.service.js",
                      "~/js/app.service.js",
                      "~/js/app.idm.service.js",
                      "~/js/user.service.js",
                      "~/js/message.service.js",
                      "~/js/budget.controller.js",
                      "~/js/budgetmt.controller.js",
                      "~/js/footer.controller.js",
                      "~/js/payment.controller.js",
                      "~/js/user.controller.js",
                      "~/js/report.controller.js",
                      "~/js/admin.controller.js",
                      "~/js/overtime.controller.js"
                  ));

            bundles.Add(new ScriptBundle("~/bundles/ui-grid")
                    .Include(
                        "~/Scripts/angular-ui-grid/ui-grid.min.js",
                        "~/Scripts/lodash.min.js",
                        "~/Scripts/jszip.min.js",
                        "~/Scripts/excel-builder.dist.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/datatable")
                    .Include(
                        "~/Scripts/DataTables/datatables.min.js"
                ));
        }
    }
}
