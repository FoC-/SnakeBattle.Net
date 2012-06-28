using System.Collections.Generic;
using System.IO;
using SnakeBattleNet.Core;

namespace SnakeBattleNet.Persistance
{
    public interface IMongoGateway
    {
        ISnake GetById(string snakeId);
        IEnumerable<ISnake> GetByOwnerId(string ownerId, out int total);
        IEnumerable<ISnake> GetAll(out int total);
        void AddSnake(ISnake snake);
        void UpdateSnake(ISnake snake);
        void RemoveSnake(string snakeId);

        Stream ReadFile(string id);
        void SaveFile(string id, string fileName, Stream content, string contentType);
        void DeleteFile(string id);
    }
}