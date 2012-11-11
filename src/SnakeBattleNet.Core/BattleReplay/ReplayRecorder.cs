using System;
using System.Collections.Generic;
using System.Linq;
using SnakeBattleNet.Core.Battlefield;
using SnakeBattleNet.Core.Common;

namespace SnakeBattleNet.Core.BattleReplay
{
    public class ReplayRecorder : IReplayRecorder
    {
        private string[,] field;
        private Size fieldSize;
        private int randomSeed;
        private List<SnakeShort> snakes = new List<SnakeShort>();
        private List<FieldEvent> events = new List<FieldEvent>();


        public void InitBattleField(IBattleField battleField)
        {
            fieldSize = battleField.Size;
            field = new string[battleField.Size.X, battleField.Size.Y];
            for (int y = 0; y < battleField.Size.Y; y++)
            {
                for (int x = 0; x < battleField.Size.X; x++)
                {
                    var fieldRow = battleField[x, y];
                    switch (fieldRow.FieldRowContent)
                    {
                        case FieldRowContent.Empty:
                            field[x, y] = "E";
                            break;
                        case FieldRowContent.Wall:
                            field[x, y] = "W";
                            break;
                        case FieldRowContent.Head:
                            field[x, y] = "H" + GetSnake(fieldRow.Id);
                            break;
                        case FieldRowContent.Body:
                            field[x, y] = "B" + GetSnake(fieldRow.Id);
                            break;
                        case FieldRowContent.Tail:
                            field[x, y] = "T" + GetSnake(fieldRow.Id);
                            break;
                    }
                }
            }
        }

        public void InitSeed(int randomSeed)
        {
            this.randomSeed = randomSeed;
        }

        public void InitSnakes(IEnumerable<ISnake> snakes)
        {
            foreach (var snake in snakes)
                GetSnake(snake.Id);
        }

        public void AddEvent(string id, int x, int y, Command command)
        {
            switch (command)
            {
                case Command.PutHead:
                    events.Add(new FieldEvent(x, y, "H" + GetSnake(id)));
                    break;
                case Command.PutBody:
                    events.Add(new FieldEvent(x, y, "B" + GetSnake(id)));
                    break;
                case Command.PutTail:
                    events.Add(new FieldEvent(x, y, "T" + GetSnake(id)));
                    break;
                case Command.PutEmpty:
                    events.Add(new FieldEvent(x, y, "E"));
                    break;
                case Command.PutGateway:
                    events.Add(new FieldEvent(x, y, "G"));
                    break;
            }
        }

        public Dictionary<string, object> GetReplay()
        {
            var objects = new Dictionary<string, object>();
            objects.Add("snakes", snakes);
            objects.Add("field", field);
            objects.Add("fieldSize", fieldSize);
            objects.Add("randomSeed", randomSeed);
            objects.Add("events", events);
            return objects;
        }

        private int GetSnake(string id)
        {
            foreach (var snakeShort in this.snakes.Where(snake => snake.LongId == id))
                return snakeShort.ShortId;

            int c = snakes.Count + 1;
            this.snakes.Add(new SnakeShort(id, c));
            return c;
        }
    }

    class FieldEvent
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string T { get; set; }
        public FieldEvent(int x, int y, string t)
        {
            X = x;
            Y = y;
            T = t;
        }
    }

    class SnakeShort
    {
        public string LongId { get; set; }
        public int ShortId { get; set; }

        public SnakeShort(string longId, int shortId)
        {
            LongId = longId;
            ShortId = shortId;
        }
    }
}