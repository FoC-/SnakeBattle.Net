using System;
using System.Collections.Generic;
using System.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SnakeBattleNet.Web.Core.Auth;
using SnakeBattleNet.Web.Utils;
using StructureMap;

namespace SnakeBattleNet.Web.DependencyResolution.Providers
{
    public class MongoProviders
    {
        private static readonly Dictionary<Type, Action<object>> IndexEnsurers = new Dictionary<Type, Action<object>>
            {
                {typeof (MongoCollection<UserIdentity>), EnsureUserIdentity}
            };


        public static MongoDatabase ProvideDatabase()
        {
            MapIdentities();

            var connectionString = ConfigurationManager.AppSettings.Get("MONGOLAB_URI") ??
                                   "mongodb://localhost/SnakeBattle";
            var mongoUrl = new MongoUrl(connectionString);
            var server = new MongoClient(mongoUrl).GetServer();
            return server.GetDatabase(mongoUrl.DatabaseName);
        }

        public static object ProvideCollection(IContext c)
        {
            var requestedType = c.BuildStack.Current.RequestedType;
            var type = requestedType.GetGenericArguments()[0];
            var database = c.GetInstance<MongoDatabase>();
            var collection = database.GetCollection(type, type.Name);
            
            Action<object> ensure;
            if (IndexEnsurers.TryGetValue(collection.GetType(), out ensure)) ensure(collection);

            return collection;
        }

        private static void MapIdentities()
        {
            if (BsonClassMap.IsClassMapRegistered(typeof(UserIdentity))) return;
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

        private static void EnsureUserIdentity(object collection)
        {
            var userIdentity = (MongoCollection<UserIdentity>)collection;
            userIdentity.EnsureIndex(Util.GetElementNameFor<UserIdentity>(_ => _.UserName));
        }
    }
}