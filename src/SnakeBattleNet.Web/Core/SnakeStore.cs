using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Implementation;
using SnakeBattleNet.Web.Utils;

namespace SnakeBattleNet.Web.Core
{
    public class SnakeStore : ISnakeStore
    {
        private readonly MongoCollection<Snake> _snakesCollection;

        public SnakeStore(MongoCollection<Snake> snakesCollection)
        {
            _snakesCollection = snakesCollection;
        }

        public ISnake GetById(string snakeId)
        {
            return snakeId.IsNullOrEmpty() ? null : _snakesCollection.FindOneById(snakeId);
        }

        public IEnumerable<ISnake> GetByOwnerId(string ownerId)
        {
            return _snakesCollection.AsQueryable().Where(_ => _.OwnerId == ownerId);
        }

        public IEnumerable<ISnake> GetAll()
        {
            return _snakesCollection.AsQueryable().ToList();
        }

        public void SaveSnake(ISnake snake)
        {
            _snakesCollection.Save(snake);
        }

        public void RemoveSnake(string snakeId)
        {
            var query = new QueryDocument();
            query.AddRange(new Dictionary<string, object>
            {
                {"_id", snakeId}
            });

            _snakesCollection.Remove(query);
        }
    }
}