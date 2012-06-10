using System.Collections.Generic;
using System.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using SnakeBattleNet.Core.Implementation;
using SnakeBattleNet.Core.Snake;
using SnakeBattleNet.Core.Snake.Implementation;

namespace SnakeBattleNet.Persistance
{
    public class MongoGateway<T>
    {
        private readonly string collectionName = typeof(T).Name;

        private static string GetMongoDbConnectionString()
        {
            return ConfigurationManager.AppSettings.Get("MONGOLAB_URI") ?? "mongodb://localhost/SnakeBattle";
        }
        private static MongoDatabase DataBase
        {
            get { return MongoDatabase.Create(GetMongoDbConnectionString()); }
        }
        private static MongoCollection<T> Collection
        {
            get { return DataBase.GetCollection<T>(typeof(T).Name); }
        }

        static MongoGateway()
        {
            RegisterClassMapping();
        }

        private static void RegisterClassMapping()
        {
            BsonClassMap.RegisterClassMap<Snake>(cm =>       { cm.AutoMap(); cm.SetDiscriminator("Snake");       cm.SetIgnoreExtraElements(true); cm.UnmapProperty(c => c.BodyParts); });
            BsonClassMap.RegisterClassMap<BrainModule>(cm => { cm.AutoMap(); cm.SetDiscriminator("BrainModule"); cm.SetIgnoreExtraElements(true); });
            BsonClassMap.RegisterClassMap<ModuleRow>(cm =>   { cm.AutoMap(); cm.SetDiscriminator("ModuleRow");   cm.SetIgnoreExtraElements(true); });
        }

        public T Get(string id)
        {
            return Collection.FindOneById(id);
        }

        public IEnumerable<T> GetAll()
        {
            return Collection.FindAll();
        }

        public void Add(T o)
        {
            Collection.Insert(o);
        }

        public void Update(T o)
        {
            Collection.Save(o);
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
