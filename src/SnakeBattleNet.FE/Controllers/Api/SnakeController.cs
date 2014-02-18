using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SnakeBattleNet.Core;
using SnakeBattleNet.Core.Common;
using SnakeBattleNet.Core.Implementation;
using SnakeBattleNet.Core.Snake;
using SnakeBattleNet.Core.Snake.Implementation;
using SnakeBattleNet.FE.Models.Snakes;
using SnakeBattleNet.Persistance;

namespace SnakeBattleNet.FE.Controllers.Api
{
    [Authorize]
    public class SnakeController : ApiController
    {
        private string NewId { get { return Guid.NewGuid().ToString().Replace("-", "").ToLower(); } }
        private string UserId { get { return User.Identity.Name; } }
        private readonly IMongoGateway _mongoGateway;

        public SnakeController(IMongoGateway mongoGateway)
        {
            _mongoGateway = mongoGateway;
        }

        public BrainedSnakeModel Get(string id)
        {
            var snake = GetSnake(id);
            return Map(snake);
        }

        public IEnumerable<SnakeModel> GetAllSnakesForCurrentUser()
        {
            int total;
            var snakes = _mongoGateway.GetByOwnerId(UserId, out total);
            return snakes.Select(Map);
        }

        public void Post([FromBody]BrainedSnakeModel model)
        {
            var snake = Map(model);
            if (snake.BrainModules.Count() > snake.ModulesMax)
            {
                throw new Exception("Incorrect number of brain modules");
            }
            _mongoGateway.SaveSnake(snake);
        }

        public void Delete(string id)
        {
            var snake = GetSnake(id);
            if (snake != null)
            {
                _mongoGateway.RemoveSnake(snake.Id);
            }
        }

        private ISnake GetSnake(string id)
        {
            var snake = _mongoGateway.GetSnakeById(id);
            if (snake == null || snake.OwnerId != UserId)
            {
                return null;
            }
            return snake;
        }

        private static BrainedSnakeModel Map(ISnake snake)
        {
            var brainedSnakeModel = new BrainedSnakeModel
            {
                Id = snake.Id,
                SnakeName = snake.SnakeName,
                Created = snake.Created,
                Wins = snake.Wins,
                Loses = snake.Loses,
                Score = snake.Score,
                Matches = snake.Matches,
                ModulesMax = snake.ModulesMax,
                VisionRadius = snake.VisionRadius,
            };
            foreach (var module in snake.BrainModules)
            {
                var item = new BrainModuleModel();
                for (var x = 0; x < module.Size.X; x++)
                    for (var y = 0; y < module.Size.Y; y++)
                    {
                        var row = module[x, y];
                        item.Rows.Add(new BrainModuleRowModel
                        {
                            X = x,
                            Y = y,
                            Color = row.AoColor.ToString(),
                            Element = row.ModuleRowContent.ToString(),
                            Excluded = row.Exclude == Exclude.Yes,
                        });
                    }
                brainedSnakeModel.BrainModuleModels.Add(item);
            }

            return brainedSnakeModel;
        }

        private ISnake Map(BrainedSnakeModel snake)
        {
            var mergedSnake = GetSnake(snake.Id) ?? new Snake(NewId, UserId);
            mergedSnake.SnakeName = snake.SnakeName;
            var brainModules = new List<IBrainModule>();
            foreach (var brainModuleModel in snake.BrainModuleModels)
            {
                var brainModule = new BrainModule(new Size(7, 7), mergedSnake.Id);
                foreach (var row in brainModuleModel.Rows.Where(row => row.X <= 7 && row.X >= 0 && row.Y <= 7 && row.Y >= 0))
                {
                    switch (row.Element)
                    {
                        case "ownHead":
                            brainModule.SetOwnHead(row.X, row.Y, ToColor(row.Color), Direction.North);
                            break;
                        case "ownBody":
                            brainModule.SetOwnBody(row.X, row.Y, ToExcluded(row.Excluded), ToColor(row.Color));
                            break;
                        case "ownTail":
                            brainModule.SetOwnTail(row.X, row.Y, ToExcluded(row.Excluded), ToColor(row.Color));
                            break;
                        case "enemyHead":
                            brainModule.SetEnemyHead(row.X, row.Y, ToExcluded(row.Excluded), ToColor(row.Color));
                            break;
                        case "enemyBody":
                            brainModule.SetEnemyBody(row.X, row.Y, ToExcluded(row.Excluded), ToColor(row.Color));
                            break;
                        case "enemyTail":
                            brainModule.SetEnemyTail(row.X, row.Y, ToExcluded(row.Excluded), ToColor(row.Color));
                            break;
                        case "empty":
                            brainModule.SetEmpty(row.X, row.Y, ToExcluded(row.Excluded), ToColor(row.Color));
                            break;
                        case "undefinied":
                            brainModule.SetUndefinied(row.X, row.Y);
                            break;
                        case "wall":
                            brainModule.SetWall(row.X, row.Y, ToExcluded(row.Excluded), ToColor(row.Color));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("element");
                    }
                }
                brainModules.Add(brainModule);
            }
            mergedSnake.BrainModules = brainModules;

            return mergedSnake;
        }

        private AOColor ToColor(string color)
        {
            switch (color)
            {
                case "OrBlue":
                    return AOColor.OrBlue;
                case "OrGreen":
                    return AOColor.OrGreen;
                case "AndGrey":
                    return AOColor.AndGrey;
                case "AndRed":
                    return AOColor.AndRed;
                case "AndBlack":
                    return AOColor.AndBlack;
                default:
                    throw new ArgumentOutOfRangeException("color");
            }
        }

        private static Exclude ToExcluded(bool excluded)
        {
            return excluded ? Exclude.Yes : Exclude.No;
        }
    }
}
