using System.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using SnakeBattleNet.Core.Implementation;
using SnakeBattleNet.Core.Snake;
using SnakeBattleNet.Core.Snake.Implementation;

namespace SnakeBattleNet.Persistance
{
    public class MongoGatewayBase<T>
    {
        protected readonly string collectionName = typeof(T).Name;

        protected static string GetMongoDbConnectionString()
        {
            return ConfigurationManager.AppSettings.Get("MONGOLAB_URI") ?? "mongodb://localhost/SnakeBattle";
        }
        protected static MongoDatabase DataBase
        {
            get { return MongoDatabase.Create(GetMongoDbConnectionString()); }
        }
        public static MongoCollection<T> Collection
        {
            get { return DataBase.GetCollection<T>(typeof(T).Name); }
        }
        public static MongoGridFS GridFS
        {
            get { return DataBase.GridFS; }
        }

        static MongoGatewayBase()
        {
            RegisterClassMapping();
        }

        protected static void RegisterClassMapping()
        {
            BsonClassMap.RegisterClassMap<Snake>(cm => { cm.AutoMap(); cm.SetDiscriminator("Snake"); cm.SetIgnoreExtraElements(true); cm.UnmapProperty(c => c.BodyParts); });
            BsonClassMap.RegisterClassMap<BrainModule>(cm => { cm.AutoMap(); cm.SetDiscriminator("BrainModule"); cm.SetIgnoreExtraElements(true); });
            BsonClassMap.RegisterClassMap<ModuleRow>(cm => { cm.AutoMap(); cm.SetDiscriminator("ModuleRow"); cm.SetIgnoreExtraElements(true); });
        }

        public void CreateIndex(params string[] indexedFields)
        {
            if (!DataBase.CollectionExists(this.collectionName))
                DataBase.CreateCollection(this.collectionName);

            if (!Collection.IndexExists(IndexKeys.Ascending(indexedFields)))
                Collection.CreateIndex(IndexKeys.Ascending(indexedFields));
        }
    }
}