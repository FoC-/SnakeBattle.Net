using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;
using SnakeBattleNet.Core.Implementation;
using SnakeBattleNet.Core.Snake;
using SnakeBattleNet.Utils.Extensions;
using SnakeBattleNet.Core.Snake.Implementation;

namespace SnakeBattleNet.Persistance
{
    public class MongoGateway : IMongoGateway
    {
        private static string GetMongoDbConnectionString()
        {
            return ConfigurationManager.AppSettings.Get("MONGOLAB_URI") ?? "mongodb://localhost/SnakeBattle";
        }
        private static MongoDatabase DataBase
        {
            get { return MongoDatabase.Create(GetMongoDbConnectionString()); }
        }
        private static MongoCollection<Snake> SnakesCollection
        {
            get { return DataBase.GetCollection<Snake>(typeof(Snake).Name); }
        }
        private static MongoGridFS GridFS
        {
            get { return DataBase.GridFS; }
        }

        static MongoGateway()
        {
            RegisterClassMapping();
            CreateIndex();
        }

        private static void RegisterClassMapping()
        {
            BsonClassMap.RegisterClassMap<Snake>(cm => { cm.AutoMap(); cm.SetDiscriminator("Snake"); cm.SetIgnoreExtraElements(true); cm.UnmapProperty(c => c.BodyParts); });
            BsonClassMap.RegisterClassMap<BrainModule>(cm => { cm.AutoMap(); cm.SetDiscriminator("BrainModule"); cm.SetIgnoreExtraElements(true); });
            BsonClassMap.RegisterClassMap<ModuleRow>(cm => { cm.AutoMap(); cm.SetDiscriminator("ModuleRow"); cm.SetIgnoreExtraElements(true); });
        }
        private static void CreateIndex()
        {
            SnakesCollection.EnsureIndex(Util.GetElementNameFor<Snake>(_ => _.Id));
            SnakesCollection.EnsureIndex(Util.GetElementNameFor<Snake>(_ => _.OwnerId));
        }

        #region Snakes
        public IEnumerable<Snake> GetByOwnerId(string ownerId, out int total)
        {
            var snakes = SnakesCollection.AsQueryable().Where(_ => _.OwnerId == ownerId);
            total = snakes.Count();

            return snakes;
        }

        public IEnumerable<Snake> GetAll(out int total)
        {
            var snakes = SnakesCollection.AsQueryable().ToList();
            total = snakes.Count;

            return snakes;
        }

        public void AddSnake(Snake snake)
        {
            SnakesCollection.Insert(snake);
        }

        public void UpdateSnake(Snake snake)
        {
            SnakesCollection.Save(snake);
        }

        public void RemoveSnake(string snakeId)
        {
            var query = new QueryDocument
            {
                new Dictionary<string, object>
                {
                    {Util.GetElementNameFor<Snake>(_ => _.Id), snakeId}
                }
            };

            SnakesCollection.Remove(query);
        }
        #endregion

        #region Textures
        public Stream ReadFile(string id)
        {
            if (id.IsNullOrEmpty())
                return null;

            MongoGridFSFileInfo file = GridFS.FindOneById(id);
            return file == null ? null : file.OpenRead();
        }

        public void SaveFile(string id, string fileName, Stream content, string contentType)
        {
            GridFS.Upload(content, fileName, new MongoGridFSCreateOptions { Id = id, ContentType = contentType });
        }

        public void DeleteFile(string id)
        {
            GridFS.DeleteById(id);
        }
        #endregion
    }
}