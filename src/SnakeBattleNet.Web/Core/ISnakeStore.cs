using System.Collections.Generic;

namespace SnakeBattleNet.Web.Core
{
    public interface ISnakeStore
    {
        Snake GetById(string snakeId);
        IEnumerable<Snake> GetAll();
        IEnumerable<Snake> GetByOwnerId(string ownerId);
        void SaveSnake(Snake snake);
        void RemoveSnake(string snakeId);
    }
}