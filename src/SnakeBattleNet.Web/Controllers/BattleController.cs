using System.Linq;
using System.Web.Http;
using AutoMapper;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;
using SnakeBattleNet.Web.Core;
using SnakeBattleNet.Web.Models.Snake;

namespace SnakeBattleNet.Web.Controllers
{
    [RoutePrefix("api/Battle")]
    public class BattleController : ApiController
    {
        private readonly ISnakeStore snakeStore;

        public BattleController(ISnakeStore snakeStore)
        {
            this.snakeStore = snakeStore;
        }

        [Route("Get")]
        public ReplayViewModel Get([FromUri]string[] ids)
        {
            var heads = new[]
            {
                new Move {X = 1, Y = 13, Direction = Direction.East},
                new Move {X = 25, Y = 13, Direction = Direction.West},
                new Move {X = 13, Y = 1, Direction = Direction.North},
                new Move {X = 13, Y = 25, Direction = Direction.South}
            };
            var n = 0;

            var battleField = new BattleField();
            var fighters = ids
                .Select(id => snakeStore.GetById(id))
                .Select(snake => new Fighter(snake.Id, battleField, snake.Chips, heads[n++]))
                .ToList();
            var replay = new Replay { BattleField = new BattleField() };
            var battleManager = new BattleManager(fighters, replay);
            battleManager.Fight(550); //original 550
            var model = Mapper.Map<Replay, ReplayViewModel>(replay);
            return model;
        }
    }
}