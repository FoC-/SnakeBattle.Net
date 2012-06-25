using System.Web.Mvc;
using System.Web.Security;

namespace SnakeBattleNet.Web.Controllers
{
    public class EditController : Controller
    {
        public ActionResult Index()
        {
            var id = GetUserId();
            return View();
        }

        public ActionResult EditName()
        {
            var id = GetUserId();
            return View();
        }

        public ActionResult UploadTexture()
        {
            var id = GetUserId();
            return View();
        }

        public ActionResult AddChip()
        {
            var id = GetUserId();
            return View();
        }

        public ActionResult EditChip()
        {
            var id = GetUserId();
            return View();
        }

        public ActionResult RemoveChip()
        {
            var id = GetUserId();
            return View();
        }

        private string GetUserId()
        {
            MembershipUser currentUser = Membership.GetUser(User.Identity.Name, userIsOnline: true);
            return currentUser == null ? null : currentUser.ProviderUserKey.ToString();
        }
    }
}
