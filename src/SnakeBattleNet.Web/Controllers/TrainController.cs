﻿using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Implementation;
using SnakeBattleNet.Web.Core;
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
            return View(snakes);
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
            return View(snake);
        }
    }
}