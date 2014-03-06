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
            var fighters = ids.Select(id => snakeStore.GetById(id)).Select(snake => new Fighter(snake.Id, snake.Chips)).ToList();
            var battleManager = new BattleManager();
            var replay = battleManager.Fight(fighters, 3);
            var model = Mapper.Map<Replay, ReplayViewModel>(replay);
            return model;
        }
    }
}