using System.Configuration;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using MongoDB.Driver;
using SnakeBattleNet.Web.Core.Auth;
using SnakeBattleNet.Web.DependencyResolution.Providers;
using StructureMap;
namespace SnakeBattleNet.Web.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
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
                        });
            return ObjectFactory.Container;
        }
    }
}