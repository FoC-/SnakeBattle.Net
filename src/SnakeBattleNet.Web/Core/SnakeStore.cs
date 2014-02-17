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

        public IEnumerable<ISnake> GetByOwnerId(string ownerId, out int total)
        {
            var snakes = _snakesCollection.AsQueryable().Where(_ => _.OwnerId == ownerId);
            total = snakes.Count();

            return snakes;
        }

        public IEnumerable<ISnake> GetAll(out int total)
        {
            var snakes = _snakesCollection.AsQueryable().ToList();
            total = snakes.Count;

            return snakes;
        }

        public void AddSnake(ISnake snake)
        {
            _snakesCollection.Insert(snake);
        }

        public void UpdateSnake(ISnake snake)
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