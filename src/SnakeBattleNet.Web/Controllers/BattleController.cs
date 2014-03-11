using System.Linq;
using System.Web.Http;
using AutoMapper;
using SnakeBattleNet.Core;
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
            var battleField = new BattleField();
            var fighters = ids.Select(id => snakeStore.GetById(id)).Select(snake => new Fighter(snake.Id, battleField, snake.Chips)).ToList();
            var replay = new Replay { BattleField = new BattleField() };
            var battleManager = new BattleManager(fighters, replay);
            battleManager.Fight(550); //original 550
            var model = Mapper.Map<Replay, ReplayViewModel>(replay);
            return model;
        }
    }
}