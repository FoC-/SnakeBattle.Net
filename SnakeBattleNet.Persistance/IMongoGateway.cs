using System.Collections.Generic;
using System.IO;
using SnakeBattleNet.Core.Implementation;

namespace SnakeBattleNet.Persistance
{
    public interface IMongoGateway
    {
        Snake GetById(string snakeId);
        IEnumerable<Snake> GetByOwnerId(string ownerId, out int total);
        IEnumerable<Snake> GetAll(out int total);
        void AddSnake(Snake snake);
        void UpdateSnake(Snake snake);
        void RemoveSnake(string snakeId);

        Stream ReadFile(string id);
        void SaveFile(string id, string fileName, Stream content, string contentType);
        void DeleteFile(string id);
    }
}