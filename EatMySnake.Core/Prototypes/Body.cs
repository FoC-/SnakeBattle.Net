using System;
using System.Collections.Generic;

namespace EatMySnake.Core.Prototypes
{
    internal class Body
    {
        private LinkedList<BodyPart> bodyParts;

        public int Length { get; set; }
        public BodyPart Head
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public BodyPart Tail
        {
            get
            {
                throw new NotImplementedException();
            }
        }

    }

    internal struct BodyPart
    {
        public Coordinate Coordinate { get; set; }
    }
}