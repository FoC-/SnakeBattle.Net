using System;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using SnakeBattleNet.Web.Core;
using SnakeBattleNet.Web.Models.Snake;

namespace SnakeBattleNet.Web.Controllers.Api
{
    [Authorize]
    [RoutePrefix("api/Snake")]
    public class SnakeController : ApiController
    {
        private readonly ISnakeStore snakeStore;

        public SnakeController(ISnakeStore snakeStore)
        {
            this.snakeStore = snakeStore;
        }
        [Route("Get")]
        public SnakeViewModel Get(string id)
        {
            var snake = snakeStore.GetById(id);
            if (!IsCurrentUserOwner(snake))
            {
                return new SnakeViewModel();
            }
            var model = Mapper.Map<Snake, SnakeViewModel>(snake);
            return model;
        }

        [Route("Save")]
        public IHttpActionResult Save(SnakeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var snakeStored = snakeStore.GetById(model.Id);
            if (!IsCurrentUserOwner(snakeStored))
            {
                ModelState.AddModelError("", "You are not owner for this snake");
                return BadRequest(ModelState);
            }

            if (!model.Chips.All(c => c.Any(m => m.IsSelf && m.C == SnakeBattleNet.Core.Contract.Content.Head)))
            {
                ModelState.AddModelError("", "Each chip should contain own head");
                return BadRequest(ModelState);
            }

            var snakemapped = Mapper.Map<Tuple<Snake, SnakeViewModel>, Snake>(new Tuple<Snake, SnakeViewModel>(snakeStored, model));
            snakeStore.SaveSnake(snakemapped);
            return Ok();
        }

        private bool IsCurrentUserOwner(Snake snake)
        {
            return snake.OwnerId == User.Identity.GetUserId();
        }
    }
}
