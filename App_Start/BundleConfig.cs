using SquishIt.Framework;

namespace DevSumScheduler
{
    public static class BundleConfig
    {
        public static void RegisterBundles()
        {
            Bundle.Css().Add("~/content/site.css").AsCached("site", "~/content/min/#/site.css");

            Bundle.JavaScript().Add("~/content/site.js").AsCached("site", "~/content/min/#/site.js");
        }
    }
}