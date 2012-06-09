using System.Web.Mvc;

namespace SnakeBattleNet.Mvc.Controllers
{
    public class EditController : Controller
    {
        public ActionResult Index()
        {
            //return snake (chips score)
            return View();
        }

        public ActionResult EditName()
        {
            return View();
        }

        public ActionResult UploadTextures()
        {
            return View();
        }

        public ActionResult AddChip()
        {
            return View();
        }

        public ActionResult EditChip()
        {
            return View();
        }

        public ActionResult RemoveChip()
        {
            return View();
        }
    }
}
