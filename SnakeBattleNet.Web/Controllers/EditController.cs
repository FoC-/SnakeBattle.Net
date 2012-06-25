using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using SnakeBattleNet.Core.Implementation;
using SnakeBattleNet.Persistance;

namespace SnakeBattleNet.Web.Controllers
{
    [Authorize]
    public class EditController : Controller
    {
        private IMongoGateway mongoGateway;

        protected override void Initialize(RequestContext requestContext)
        {
            mongoGateway = new MongoGateway();
            base.Initialize(requestContext);
        }

        public ActionResult Index(string snakeId)
        {
            var snake = mongoGateway.GetById(snakeId);

            CheckOwner(snake);

            return View(snake);
        }

        public ActionResult EditName(string snakeId)
        {
            var snake = mongoGateway.GetById(snakeId);
            return View();
        }

        public ActionResult UploadTexture(string snakeId)
        {
            var id = GetUserId();
            return View();
        }

        public ActionResult AddChip(string snakeId)
        {
            var snake = mongoGateway.GetById(snakeId);
            return View();
        }

        public ActionResult EditChip(string snakeId)
        {
            var snake = mongoGateway.GetById(snakeId);
            return View();
        }

        public ActionResult RemoveChip(string snakeId)
        {
            var snake = mongoGateway.GetById(snakeId);
            return View();
        }

        private string GetUserId()
        {
            MembershipUser currentUser = Membership.GetUser(User.Identity.Name, userIsOnline: true);
            return currentUser == null ? null : currentUser.ProviderUserKey.ToString();
        }

        private void CheckOwner(Snake snake)
        {
            var userId = GetUserId();
            if (snake == null || snake.OwnerId != userId)
            {
                ModelState.AddModelError("", "This warrior is not belong to you");
            }
        }
    }
}
