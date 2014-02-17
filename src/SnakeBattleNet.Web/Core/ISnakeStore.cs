using System.Collections.Generic;
using SnakeBattleNet.Core;

namespace SnakeBattleNet.Web.Core
{
    public interface ISnakeStore
    {
        ISnake GetById(string snakeId);
        IEnumerable<ISnake> GetByOwnerId(string ownerId, out int total);
        IEnumerable<ISnake> GetAll(out int total);
        void AddSnake(ISnake snake);
        void UpdateSnake(ISnake snake);
        void RemoveSnake(string snakeId);
    }
}