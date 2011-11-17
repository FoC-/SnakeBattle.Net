using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EatMySnake.Core.Snake;

namespace EatMySnake.Core.Logic
{
    class Core
    {

        public Move NextMove(Matrix currentStateOfBattleField)
        {
            //Check if movement is possible
            //if (CheckPossibleMoves(currentStateOfBattleField)[0] == Direction.NoWay)
            //{
            //    //Return current position of head
            //    return new Move();
            //}

            Snake.Snake snake = new Snake.Snake();
            

            //After calculation we should return new positon of head
            //add leangth if any tail is eaten or add new head and delete tail);
            return new Move();
        }

        //private List<Direction> CheckPossibleMoves(Matrix currentStateOfBattleField)
        //{
        //    List<Direction> directions = new List<Direction>();
        //    int headX = BodyParts[0].x;
        //    int headY = BodyParts[0].y;
        //    if (currentStateOfBattleField.Rows[headX, headY + 1].Content == Content.Empty
        //        || currentStateOfBattleField.Rows[headX, headY + 1].Content == Content.EnemyTail
        //        || currentStateOfBattleField.Rows[headX, headY + 1].Content == Content.OwnTail)
        //        directions.Add(Direction.North);
        //    if (currentStateOfBattleField.Rows[headX - 1, headY].Content == Content.Empty
        //        || currentStateOfBattleField.Rows[headX - 1, headY].Content == Content.EnemyTail
        //        || currentStateOfBattleField.Rows[headX - 1, headY].Content == Content.OwnTail)
        //        directions.Add(Direction.South);
        //    if (currentStateOfBattleField.Rows[headX - 1, headY].Content == Content.Empty
        //        || currentStateOfBattleField.Rows[headX - 1, headY].Content == Content.EnemyTail
        //        || currentStateOfBattleField.Rows[headX - 1, headY].Content == Content.OwnTail)
        //        directions.Add(Direction.West);
        //    if (currentStateOfBattleField.Rows[headX + 1, headY].Content == Content.Empty
        //        || currentStateOfBattleField.Rows[headX + 1, headY].Content == Content.EnemyTail
        //        || currentStateOfBattleField.Rows[headX + 1, headY].Content == Content.OwnTail)
        //        directions.Add(Direction.East);
        //    if (directions.Count == 0)
        //    {
        //        directions.Add(Direction.NoWay);
        //    }
        //    return directions;
        //}
    }
}
