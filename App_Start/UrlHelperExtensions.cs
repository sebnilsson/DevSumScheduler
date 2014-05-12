using System.Web.Mvc;

namespace DevSumScheduler
{
    public static class UrlHelperExtensions
    {
        public static string Home(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("Home", new { action = "Index" });
        }

        public static string About(this UrlHelper urlHelper)
        {
            return urlHelper.RouteUrl("Home", new { action = "About" });
        }
    }
}