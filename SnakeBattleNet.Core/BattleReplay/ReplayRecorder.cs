using System;
using System.Collections.Generic;
using SnakeBattleNet.Core.Battlefield;
using SnakeBattleNet.Core.Common;
using SnakeBattleNet.Utils.Extensions;

namespace SnakeBattleNet.Core.BattleReplay
{
    public class ReplayRecorder : IReplayRecorder
    {
        private string[,] field;
        private Size fieldSize;
        private Dictionary<string, int> snakes;
        private int RandomSeed;
        private List<string> events = new List<string>();


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
            RandomSeed = randomSeed;
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
                    events.Add("[x:{0},y:{1},t:'H{2}']".F(x, y, GetSnake(id.Value)));
                    break;
                case Command.PutBody:
                    events.Add("[x:{0},y:{1},t:'B{2}']".F(x, y, GetSnake(id.Value)));
                    break;
                case Command.PutTail:
                    events.Add("[x:{0},y:{1},t:'T{2}']".F(x, y, GetSnake(id.Value)));
                    break;
                case Command.PutEmpty:
                    events.Add("[x:{0},y:{1},t:'E']".F(x, y));
                    break;
                case Command.PutGateway:
                    events.Add("[x:{0},y:{1},t:'G']".F(x, y));
                    break;
            }
        }

        public Dictionary<string, object> GetReplay()
        {
            var objects = new Dictionary<string, object>();
            objects.Add("field", field);
            objects.Add("fieldSize", fieldSize);
            objects.Add("snakes", snakes);
            objects.Add("events", events);
            return objects;
        }

        private int GetSnake(Guid id)
        {
            if (snakes == null)
                snakes = new Dictionary<string, int>();

            var key = id.ToString();

            if (snakes.ContainsKey(key))
                return snakes[key];

            int c = snakes.Count;
            snakes.Add(key, c);

            return c;
        }
    }
}