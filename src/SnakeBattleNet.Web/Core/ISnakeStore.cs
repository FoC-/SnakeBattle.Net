using System.Collections.Generic;
using SnakeBattleNet.Core;

namespace SnakeBattleNet.Web.Core
{
    public interface ISnakeStore
    {
        ISnake GetById(string snakeId);
        IEnumerable<ISnake> GetAll();
        IEnumerable<ISnake> GetByOwnerId(string ownerId);
        void SaveSnake(ISnake snake);
        void RemoveSnake(string snakeId);
    }
}