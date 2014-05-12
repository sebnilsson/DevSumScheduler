using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace DevSumScheduler
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.AppendTrailingSlash = true;
            routes.LowercaseUrls = true;
            routes.RouteExistingFiles = true;

            routes.MapMvcAttributeRoutes();
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(
                x =>
                    {
                        x.MapHttpAttributeRoutes();

                        x.Formatters.Add(new HtmlFormatter());
                    });

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles();
        }
    }
}