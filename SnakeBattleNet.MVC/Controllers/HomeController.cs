using System.Web.Mvc;
using SnakeBattleNet.MVC.Models;

namespace SnakeBattleNet.MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var battleField = @"{
                                    field: {
                                        x: 27,
                                        y: 27
                                    },
                                    segments: [{
                                        x: 75,
                                        y: 75
                                    }]
                                }";
            string textures = "{field: 'Content/field.bmp',snakes: [\"Content/snake.bmp\",\"Content/snake.bmp\"]}";
            var battle = new BattleDto() { BattleFieldJson = battleField, TexturesForSnakesJson = textures };

            return View(battle);
        }

    }
}
