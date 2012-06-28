using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Implementation;
using SnakeBattleNet.Persistance;
using SnakeBattleNet.Web.Models;

namespace SnakeBattleNet.Web.Controllers
{
    [Authorize]
    public class TrainingController : Controller
    {
        private const int Max_Number_Per_User = 2;
        private IMongoGateway mongoGateway;
        private string CurrentUserId
        {
            get
            {
                MembershipUser currentUser = Membership.GetUser(User.Identity.Name, userIsOnline: true);
                return currentUser == null ? null : currentUser.ProviderUserKey.ToString();
            }
        }

        protected override void Initialize(RequestContext requestContext)
        {
            mongoGateway = new MongoGateway();
            base.Initialize(requestContext);
        }

        public ActionResult Index()
        {
            int number;
            IEnumerable<ISnake> snakes = this.mongoGateway.GetByOwnerId(CurrentUserId, out number);
            bool canAdd = number < Max_Number_Per_User;

            var stats = snakes.Select(snake => new SnakeStatsViewModel(snake.Id, snake.SnakeName, snake.Wins, snake.Loses, snake.Matches, snake.Score)).ToList();

            var trainingViewModel = new TrainingViewModel
            {
                CanAdd = canAdd,
                SnakeStats = stats
            };

            return number == 0 ? View() : View(trainingViewModel);
        }

        public ActionResult AddSnake()
        {
            var snake = new Snake(Guid.NewGuid().ToString(), CurrentUserId);
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
            var snake = mongoGateway.GetById(snakeId);

            var snakeViewModel = new SnakeViewModelBase(snake.Id, snake.SnakeName);
            return View(snakeViewModel);
        }

        [HttpPost]
        public ActionResult RemoveSnake(SnakeViewModelBase model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = CurrentUserId;
            var snake = mongoGateway.GetById(model.Id);

            if (snake.OwnerId == userId)
            {
                mongoGateway.RemoveSnake(model.Id);
            }
            return RedirectToAction("Index");
        }
    }
}
