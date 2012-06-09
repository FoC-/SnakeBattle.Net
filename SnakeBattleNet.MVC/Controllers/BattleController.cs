using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.BattleReplay;
using SnakeBattleNet.Core.Battlefield;
using SnakeBattleNet.Core.Battlefield.Implementation;
using SnakeBattleNet.Core.Battlemanager;
using SnakeBattleNet.Core.Implementation;
using SnakeBattleNet.Core.Snake;

namespace SnakeBattleNet.Mvc.Controllers
{
    public class BattleController : Controller
    {
        [HttpPost]
        public ActionResult TestJson(string id)
        {
            IBattleField battleField = new BattleField();

            var newGuid = Guid.NewGuid();
            var brainChips = new List<IBrainChip>();
            var snakes = new List<ISnake>();
            snakes.Add(new Snake(Guid.NewGuid(), newGuid, "Snake number 0", brainChips));
            snakes.Add(new Snake(Guid.NewGuid(), newGuid, "Snake number 1", brainChips));
            snakes.Add(new Snake(Guid.NewGuid(), newGuid, "Snake number 2", brainChips));
            snakes.Add(new Snake(Guid.NewGuid(), newGuid, "Snake number 3", brainChips));

            IReplayRecorder replayRecorder = new ReplayRecorder();
            BattleManager battleManager = new BattleManager(battleField, snakes, replayRecorder);
            battleManager.InitializeField();

            for (int i = 0; i < 500; i++)
                battleManager.Act();

            var replay = Json(replayRecorder.GetReplay());

            return replay;
        }
    }
}
