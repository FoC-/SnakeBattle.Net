using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EatMySnake.Core.Logic;

namespace EatMySnake.Core.Snake
{
    class Snake
    {
        public string Name { get; private set; }
        public Guid Uid { get; private set; }

        public List<Matrix> BrainModules = new List<Matrix>();
        LinkedList<Move> BodyParts = new LinkedList<Move>();

        public bool LoadSnake(Guid uid)
        {
            Uid = uid;
            Name = Guid.NewGuid().ToString();
            return true;
        }

        public bool SaveSnake()
        {
            return true;
        }

        public void NextMove(Move newHeadPosition)
        {
            BodyParts.AddFirst(newHeadPosition);
            BodyParts.RemoveLast();
        }

        public void Bite(Move newHeadPosition)
        {
            BodyParts.AddFirst(newHeadPosition);
        }

        public void Bitten()
        {
            BodyParts.RemoveLast();
        }
    }
}
