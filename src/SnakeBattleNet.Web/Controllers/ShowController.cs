using System.Collections.Generic;
using System.Web.Mvc;

namespace SnakeBattleNet.Web.Controllers
{
    public class ShowController : Controller
    {
        public ActionResult Index()
        {
            var model = new List<string>();
            return View(model);
        }

        public ActionResult Demo()
        {
            return View();
        }
    }
}