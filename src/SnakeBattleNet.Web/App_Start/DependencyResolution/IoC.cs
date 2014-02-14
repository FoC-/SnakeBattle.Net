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
                            x.For<UserManager<UserIdentity>>().Use<UserManager<UserIdentity>>();
                            x.For<IUserStore<UserIdentity>>().Singleton().Use<CustomUserStore<UserIdentity>>();
                            x.For<IAuthenticationManager>().Use(() => HttpContext.Current.Request.GetOwinContext().Authentication);
                            
                            // Mongo
                            x.For<MongoDatabase>().Singleton().Use(MongoProviders.ProvideDatabase);
                            x.For(typeof(MongoCollection<>)).Use(MongoProviders.ProvideCollection);
                        });
            return ObjectFactory.Container;
        }
    }
}