using System.Collections.Generic;
using System.Drawing;
using System.Web.Mvc;

namespace SnakeBattleNet.MVC.Controllers
{
    public class BattleController : Controller
    {
        [HttpPost]
        public ActionResult GetBattle(string id)
        {
            var battleField = new Dictionary<string, Point>
                                  {
                                      {"field", new Point(27, 27)},
                                      {"segments", new Point(5,5)}
                                  };

            return Json(battleField);
        }
    }
}
