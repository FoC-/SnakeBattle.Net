using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using SnakeBattleNet.Core.Implementation;
using SnakeBattleNet.Persistance;

namespace SnakeBattleNet.Web.Controllers
{
    [Authorize]
    public class TrainingController : Controller
    {
        private IMongoGateway mongoGateway;

        protected override void Initialize(RequestContext requestContext)
        {
            mongoGateway = new MongoGateway();
            base.Initialize(requestContext);
        }

        public ActionResult Index()
        {
            string id = GetUserId();
            int number;
            IEnumerable<Snake> snakes = mongoGateway.GetByOwnerId(id, out number);

            return number == 0 ? View() : View(snakes);
        }

        public ActionResult AddSnake()
        {
            var userId = GetUserId();

            var snake = new Snake(Guid.NewGuid().ToString(), userId);
            snake.SetLoses(0);
            snake.SetWins(0);
            snake.SetModulesMax(9);
            snake.SetName("Snake Doe");
            snake.SetScore(1500);
            snake.SetVisionRadius(7);

            mongoGateway.AddSnake(snake);

            return RedirectToAction("Index");
        }

        public ActionResult EditSnake(string id)
        {
            return RedirectToAction("Index", "Edit", new { snakeId = id });
        }

        public ActionResult RemoveSnake(string snakeId)
        {
            var userId = GetUserId();
            var snake = mongoGateway.GetById(snakeId);

            if (snake != null && snake.OwnerId == userId)
            {
                mongoGateway.RemoveSnake(snakeId);
            }
            else
            {
                ModelState.AddModelError("", "This warrior is not belong to you");
            }
            return RedirectToAction("Index");
        }

        private string GetUserId()
        {
            MembershipUser currentUser = Membership.GetUser(User.Identity.Name, userIsOnline: true);
            return currentUser == null ? null : currentUser.ProviderUserKey.ToString();
        }
    }
}
