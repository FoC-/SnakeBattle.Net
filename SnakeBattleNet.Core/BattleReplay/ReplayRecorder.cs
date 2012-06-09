using System;
using System.Collections.Generic;
using SnakeBattleNet.Core.Battlefield;
using SnakeBattleNet.Core.Common;

namespace SnakeBattleNet.Core.BattleReplay
{
    public class ReplayRecorder : IReplayRecorder
    {
        private string[,] field;
        private Size fieldSize;
        private int randomSeed;
        private Dictionary<string, int> snakes = new Dictionary<string, int>();
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
                            field[x, y] = "H" + GetSnake(fieldRow.Guid);
                            break;
                        case FieldRowContent.Body:
                            field[x, y] = "B" + GetSnake(fieldRow.Guid);
                            break;
                        case FieldRowContent.Tail:
                            field[x, y] = "T" + GetSnake(fieldRow.Guid);
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

        public void AddEvent(Guid? id, int x, int y, Command command)
        {
            switch (command)
            {
                case Command.PutHead:
                    events.Add(new FieldEvent(x, y, "H" + GetSnake(id.Value)));
                    break;
                case Command.PutBody:
                    events.Add(new FieldEvent(x, y, "B" + GetSnake(id.Value)));
                    break;
                case Command.PutTail:
                    events.Add(new FieldEvent(x, y, "T" + GetSnake(id.Value)));
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

        private int GetSnake(Guid id)
        {
            var key = id.ToString();

            if (snakes.ContainsKey(key))
                return snakes[key];

            int c = snakes.Count + 1;
            snakes.Add(key, c);

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
}