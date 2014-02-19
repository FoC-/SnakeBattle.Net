using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SnakeBattleNet.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(Bootstrap.RegisterApis);
            Bootstrap.RegisterGlobalFilters(GlobalFilters.Filters);
            Bootstrap.RegisterRoutes(RouteTable.Routes);
            Bootstrap.RegisterBundles(BundleTable.Bundles);
            Bootstrap.RegisterMappings();
        }
    }
}
