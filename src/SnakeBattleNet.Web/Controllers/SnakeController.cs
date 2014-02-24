using System;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Contract;
using SnakeBattleNet.Web.Core;
using SnakeBattleNet.Web.Models.Snake;

namespace SnakeBattleNet.Web.Controllers
{
    //[Authorize]
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
            snake.Chips.Add(CreateChipWithAndColoredHead());
            //if (!IsCurrentUserOwner(snake))
            //{
            //    return new SnakeViewModel();
            //}
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
            //if (!IsCurrentUserOwner(snakeStored))
            //{
            //    ModelState.AddModelError("", "You are not owner for this snake");
            //    return BadRequest(ModelState);
            //}

            var snakemapped = Mapper.Map<Tuple<Snake, SnakeViewModel>, Snake>(new Tuple<Snake, SnakeViewModel>(snakeStored, model));
            snakeStore.SaveSnake(snakemapped);
            return Ok();
        }


        private static View<ChipCell> CreateChipWithAndColoredHead()
        {
            var chip = new View<ChipCell>();

            // Own snake
            chip[new Position { X = 2, Y = 2 }] = new ChipCell { Content = SnakeBattleNet.Core.Contract.Content.Head, Color = Color.AndGrey, IsSelf = true };
            chip[new Position { X = 2, Y = 3 }] = new ChipCell { Content = SnakeBattleNet.Core.Contract.Content.Body, Color = Color.OrGreen, IsSelf = true };
            chip[new Position { X = 3, Y = 3 }] = new ChipCell { Content = SnakeBattleNet.Core.Contract.Content.Body, Color = Color.OrGreen, IsSelf = true };
            chip[new Position { X = 3, Y = 2 }] = new ChipCell { Content = SnakeBattleNet.Core.Contract.Content.Tail, Color = Color.OrGreen, IsSelf = true };

            // Snake 1
            chip[new Position { X = 2, Y = 1 }] = new ChipCell { Content = SnakeBattleNet.Core.Contract.Content.Head, Color = Color.AndBlack };
            chip[new Position { X = 2, Y = 0 }] = new ChipCell { Content = SnakeBattleNet.Core.Contract.Content.Body, Color = Color.AndBlack };
            chip[new Position { X = 3, Y = 0 }] = new ChipCell { Content = SnakeBattleNet.Core.Contract.Content.Body, Color = Color.AndBlack };
            chip[new Position { X = 3, Y = 1 }] = new ChipCell { Content = SnakeBattleNet.Core.Contract.Content.Tail, Color = Color.AndBlack };

            // Snake 2
            chip[new Position { X = 0, Y = 0 }] = new ChipCell { Content = SnakeBattleNet.Core.Contract.Content.Head, Color = Color.AndGrey };
            chip[new Position { X = 1, Y = 0 }] = new ChipCell { Content = SnakeBattleNet.Core.Contract.Content.Body, Color = Color.AndGrey };
            chip[new Position { X = 1, Y = 1 }] = new ChipCell { Content = SnakeBattleNet.Core.Contract.Content.Body, Color = Color.AndGrey };
            chip[new Position { X = 0, Y = 1 }] = new ChipCell { Content = SnakeBattleNet.Core.Contract.Content.Body, Color = Color.AndGrey };
            chip[new Position { X = 0, Y = 2 }] = new ChipCell { Content = SnakeBattleNet.Core.Contract.Content.Tail, Color = Color.AndGrey };

            // Snake 3
            chip[new Position { X = 0, Y = 3 }] = new ChipCell { Content = SnakeBattleNet.Core.Contract.Content.Head, Color = Color.OrBlue };
            chip[new Position { X = 0, Y = 4 }] = new ChipCell { Content = SnakeBattleNet.Core.Contract.Content.Body, Color = Color.OrBlue };

            return chip;
        }

        private bool IsCurrentUserOwner(Snake snake)
        {
            return snake.OwnerId == User.Identity.GetUserId();
        }
    }
}
