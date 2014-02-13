using System.Configuration;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SnakeBattleNet.Web.Core.Auth;
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
                            // Extract this
                            x.For<MongoDatabase>().Singleton().Use(() =>
                            {
                                if (!BsonClassMap.IsClassMapRegistered(typeof(UserIdentity)))
                                {
                                    BsonClassMap.RegisterClassMap<UserIdentity>(cm =>
                                    {
                                        cm.AutoMap();
                                        cm.SetIgnoreExtraElements(true);
                                        cm.SetIsRootClass(true);
                                        cm.MapIdField(c => c.Id);
                                        cm.MapProperty(c => c.UserName).SetElementName("Username");
                                        cm.MapProperty(c => c.PasswordHash).SetElementName("PasswordHash");
                                        cm.MapProperty(c => c.Roles).SetElementName("Roles").SetIgnoreIfNull(true);
                                    });
                                }

                                var connectionString = ConfigurationManager.AppSettings.Get("MONGOLAB_URI") ??
                                                       "mongodb://localhost/SnakeBattle";
                                var mongoUrl = new MongoUrl(connectionString);
                                var server = new MongoClient(mongoUrl).GetServer();
                                return server.GetDatabase(mongoUrl.DatabaseName);
                            });
                            x.For(typeof(MongoCollection<>)).Use((c) =>
                            {
                                var requestedType = c.BuildStack.Current.RequestedType;
                                var type = requestedType.GetGenericArguments()[0];
                                var database = c.GetInstance<MongoDatabase>();
                                return database.GetCollection(type, typeof(UserIdentity).Name);
                            });
                        });
            return ObjectFactory.Container;
        }
    }
}