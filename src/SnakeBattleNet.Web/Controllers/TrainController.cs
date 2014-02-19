using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using SnakeBattleNet.Core;
using SnakeBattleNet.Web.Core;
using SnakeBattleNet.Web.Models.Snake;
using SnakeBattleNet.Web.Utils;

namespace SnakeBattleNet.Web.Controllers
{
    [Authorize]
    public class TrainController : Controller
    {
        private readonly ISnakeStore _snakeStore;

        public TrainController(ISnakeStore snakeStore)
        {
            _snakeStore = snakeStore;
        }

        public ActionResult Index()
        {
            var snakes = _snakeStore.GetByOwnerId(User.Identity.GetUserId());
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
            _snakeStore.SaveSnake(snake);
            return RedirectToAction("Edit", new { id = snake.Id });
        }

        public ActionResult Edit(string id)
        {
            if (id.IsNullOrEmpty())
            {
                return RedirectToAction("Index");
            }
            var snake = _snakeStore.GetById(id);
            var model = Mapper.Map<Snake, SnakeViewModel>(snake);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SnakeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var snakeStored = _snakeStore.GetById(model.Id);
                if (IsCurrentUserOwner(snakeStored))
                {
                    var snakemapped = Mapper.Map<Tuple<Snake, SnakeViewModel>, Snake>(new Tuple<Snake, SnakeViewModel>(snakeStored, model));
                    _snakeStore.SaveSnake(snakemapped);
                }
                else
                {
                    ModelState.AddModelError("", "You are not owner for this snake");
                }
            }
            return View(model);
        }

        private bool IsCurrentUserOwner(Snake snakeStored)
        {
            return snakeStored.OwnerId == User.Identity.GetUserId();
        }
    }
}