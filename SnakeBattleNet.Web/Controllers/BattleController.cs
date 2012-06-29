using System.Collections.Generic;
using System.Web.Mvc;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.BattleReplay;
using SnakeBattleNet.Core.Battlefield.Implementation;
using SnakeBattleNet.Core.Battlemanager;
using SnakeBattleNet.Core.Implementation;

namespace SnakeBattleNet.Web.Controllers
{
    public class BattleController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TestBattle(string id)
        {
            var owner = "Admin";
            var snakes = new List<ISnake>
            {
                new Snake("Snake1", owner),
                new Snake("Snake2", owner),
                new Snake("Snake3", owner),
                new Snake("Snake4", owner)
            };

            var battleField = new BattleField();
            var replayRecorder = new ReplayRecorder();

            var battleManager = new BattleManager(battleField, snakes, replayRecorder);
            battleManager.InitializeField();

            for (int i = 0; i < 500; i++)
                battleManager.Act();

            return Json(replayRecorder.GetReplay());
        }
    }
}
