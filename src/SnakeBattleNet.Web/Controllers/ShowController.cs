using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using SnakeBattleNet.Web.Core;
using SnakeBattleNet.Web.Models.Snake;

namespace SnakeBattleNet.Web.Controllers
{
    public class ShowController : Controller
    {
        private readonly ISnakeStore snakeStore;

        public ShowController(ISnakeStore snakeStore)
        {
            this.snakeStore = snakeStore;
        }

        public ActionResult Index()
        {
            var snakes = snakeStore.GetAll();
            var model = Mapper.Map<IEnumerable<Snake>, IEnumerable<SnakeViewModel>>(snakes);
            return View(model);
        }

        public ActionResult Go(IEnumerable<string> ids)
        {
            return View(ids.Take(4));
        }

        public ActionResult Demo()
        {
            return View();
        }
    }
}