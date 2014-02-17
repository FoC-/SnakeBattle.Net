using System.Configuration;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Newtonsoft.Json.Serialization;
using SnakeBattleNet.Web.Core;
using SnakeBattleNet.Web.Core.Auth;
using SnakeBattleNet.Web.DependencyResolution.Providers;
using StructureMap;

namespace SnakeBattleNet.Web
{
    public class Bootstrap
    {
        public static void RegisterApis(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Use camel case for JSON data.
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );
        }

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/bootstrap.css",
                 "~/Content/Site.css"));
        }

        public static IContainer RegisterDependencies()
        {
            ObjectFactory.Initialize(x =>
            {
                x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });
                x.For<IUserStore<UserIdentity>>().Singleton().Use<CustomUserStore<UserIdentity>>();
                x.For<IAuthenticationManager>().HttpContextScoped().Use(() => HttpContext.Current.Request.GetOwinContext().Authentication);

                // Mongo
                x.For<MongoDatabaseProvider>().Use<MongoDatabaseProvider>()
                    .Ctor<string>().Is(ConfigurationManager.AppSettings.Get("MONGOLAB_URI") ?? "mongodb://localhost/SnakeBattle");

                x.For<MongoDatabase>().Singleton().Use(c => c.GetInstance<MongoDatabaseProvider>().ProvideDatabase());
                x.For(typeof(MongoCollection<>)).Use(c => c.GetInstance<MongoCollectionProvider>().ProvideCollection(c));
                x.For<ISnakeStore>().Singleton().Use<SnakeStore>();
                x.For<IFileStore>().Singleton().Use<FileStore>()
                    .Ctor<MongoGridFS>().Is(c => c.GetInstance<MongoDatabase>().GridFS);
            });
            return ObjectFactory.Container;
        }
    }
}
