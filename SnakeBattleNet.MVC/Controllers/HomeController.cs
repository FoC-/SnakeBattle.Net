using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SnakeBattleNet.MVC.Models;

namespace SnakeBattleNet.MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var battleField = @"{
                                    segments: [{
                                        x: 75,
                                        y: 75
                                    }]
                                }";
            var battle = new BattleDto() { BattleFieldJson = battleField };

            return View(battle);
        }

    }
}
