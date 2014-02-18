using System.Web.Mvc;

namespace SnakeBattleNet.FE.Controllers
{
    public class SPAController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Home()
        {
            return PartialView();
        } 
        
        public ActionResult Edit()
        {
            return PartialView();
        }
    }
}
