using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SnakeBattleNet.Core.Contract;
using SnakeBattleNet.Web.Core;
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
            InitMapping();

            var mongoUrl = new MongoUrl(_connectionString);
            var server = new MongoClient(mongoUrl).GetServer();
            return server.GetDatabase(mongoUrl.DatabaseName);
        }

        private static void InitMapping()
        {
            MapUserIdentity();
            MapSnake();
        }

        private static void MapUserIdentity()
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

        private static void MapSnake()
        {
            if (BsonClassMap.IsClassMapRegistered(typeof(Snake))) return;
            BsonClassMap.RegisterClassMap<Snake>(cm =>
            {
                cm.AutoMap();
                cm.SetIsRootClass(true);
                cm.SetDiscriminator("Snake");
                cm.SetIgnoreExtraElements(true);
                cm.MapIdField(c => c.Id);
            });
            BsonClassMap.RegisterClassMap<ChipCell>(cm =>
            {
                cm.AutoMap();
                cm.SetDiscriminator("ChipCell");
                cm.SetIgnoreExtraElements(true);
            });
        }
    }
}