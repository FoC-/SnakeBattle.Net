using System;
using System.Collections.Generic;

namespace EatMySnake.Core.Prototypes
{
    public class Body
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
}