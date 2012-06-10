using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.BattleReplay;
using SnakeBattleNet.Core.Battlefield;
using SnakeBattleNet.Core.Battlefield.Implementation;
using SnakeBattleNet.Core.Battlemanager;
using SnakeBattleNet.Core.Common;
using SnakeBattleNet.Core.Implementation;
using SnakeBattleNet.Core.Snake;
using SnakeBattleNet.Core.Snake.Implementation;
using SnakeBattleNet.Persistance;

namespace SnakeBattleNet.Mvc.Controllers
{
    public class BattleController : Controller
    {
        [HttpPost]
        public ActionResult TestJson(string id)
        {
            IBattleField battleField = new BattleField();

            var newGuid = "My snake";
            var snakes = new List<ISnake>();
            snakes.Add(new Snake(Guid.NewGuid().ToString(), newGuid));
            snakes.Add(new Snake(Guid.NewGuid().ToString(), newGuid));
            snakes.Add(new Snake(Guid.NewGuid().ToString(), newGuid));
            snakes.Add(new Snake(Guid.NewGuid().ToString(), newGuid));

            IReplayRecorder replayRecorder = new ReplayRecorder();
            BattleManager battleManager = new BattleManager(battleField, snakes, replayRecorder);
            battleManager.InitializeField();

            for (int i = 0; i < 500; i++)
                battleManager.Act();

            var replay = Json(replayRecorder.GetReplay());

            return replay;
        }

        public ActionResult TestMongo()
        {
            var id = Guid.NewGuid().ToString();
            var brainModule = new BrainModule("Module-Id", new Size(5, 5), id);
            brainModule.SetEnemyHead(1, 1, Exclude.Yes, AOColor.OrGreen);

            ISnake snake = new Snake(id, "I'm owner");
            snake.SetVisionRadius(7);
            snake.SetModulesMax(2);
            snake.InsertModule(0, brainModule);

            var mongo = new MongoGateway<ISnake>();
            mongo.Add(snake);

            var sn = mongo.Get(id);

            return new EmptyResult();
        }
    }
}
