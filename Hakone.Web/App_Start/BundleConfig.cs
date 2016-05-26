using System.Web;
using System.Web.Optimization;

namespace Hakone.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                        "~/Scripts/jquery-{version}.js"
                        , "~/Scripts/jquery.lazyload.min.js"
                        , "~/Scripts/jquery.hoverIntent.minified.js"
                        , "~/Scripts/jquery.blockUI.js"
                        , "~/Scripts/layer/layer.js"
                        , "~/Scripts/jquery.validate.*"
                        ,"~/Scripts/GlobalSetting.js"
                        , "~/Scripts/site.js"
                        , "~/Scripts/shop.js"
                        , "~/Scripts/product.js"
                        ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/normalize.css",
                      "~/Content/grid.css",
                      "~/Content/site.css",
                      "~/Content/responsive.css"));
        }
    }
}
