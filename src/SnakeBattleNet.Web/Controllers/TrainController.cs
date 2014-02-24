using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using SnakeBattleNet.Web.Core;
using SnakeBattleNet.Web.Models.Snake;
using SnakeBattleNet.Web.Utils;

namespace SnakeBattleNet.Web.Controllers
{
    [Authorize]
    public class TrainController : Controller
    {
        private readonly ISnakeStore snakeStore;

        public TrainController(ISnakeStore snakeStore)
        {
            this.snakeStore = snakeStore;
        }

        public ActionResult Index()
        {
            var snakes = snakeStore.GetByOwnerId(User.Identity.GetUserId());
            var model = Mapper.Map<IEnumerable<Snake>, IEnumerable<SnakeViewModel>>(snakes);
            return View(model);
        }

        public ActionResult Add()
        {
            if (User == null)
            {
                return RedirectToAction("Index");
            }
            var snake = new Snake(User.Identity.GetUserId());
            snakeStore.SaveSnake(snake);
            return RedirectToAction("Edit", new { id = snake.Id });
        }

        public ActionResult Edit(string id)
        {
            if (id.IsNullOrEmpty())
            {
                return RedirectToAction("Index");
            }
            return View(model: id);
        }
    }
}