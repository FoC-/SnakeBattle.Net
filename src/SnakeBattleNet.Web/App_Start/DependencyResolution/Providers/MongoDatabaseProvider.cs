using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SnakeBattleNet.Web.Core.Auth;

namespace SnakeBattleNet.Web.DependencyResolution.Providers
{
    public class MongoDatabaseProvider
    {
        private readonly string _connectionString;

        public MongoDatabaseProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public MongoDatabase ProvideDatabase()
        {
            MapIdentities();

            var mongoUrl = new MongoUrl(_connectionString);
            var server = new MongoClient(mongoUrl).GetServer();
            return server.GetDatabase(mongoUrl.DatabaseName);
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
    }
}