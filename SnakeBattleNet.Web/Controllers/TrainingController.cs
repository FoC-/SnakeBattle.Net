using System.Web.Mvc;
using System.Web.Security;

namespace SnakeBattleNet.Web.Controllers
{
    [Authorize]
    public class TrainingController : Controller
    {
        public ActionResult Index()
        {
            var id = GetUserId();
            return View();
        }

        public ActionResult Snake()
        {
            var id = GetUserId();
            return View();
        }

        public ActionResult AddSnake()
        {
            var id = GetUserId();
            return View();
        }

        public ActionResult EditSnake()
        {
            var id = GetUserId();
            return View();
        }

        public ActionResult RemoveSnake()
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
