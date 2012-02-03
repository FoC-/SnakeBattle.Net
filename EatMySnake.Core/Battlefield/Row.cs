using System;
using EatMySnake.Core.Common;

namespace EatMySnake.Core.Battlefield
{
    public class Row
    {
        public Content Content { get; private set; }
        public Guid Guid { get; private set; }

        public Row() : this(Content.Empty) { }

        public Row(Content content, Guid guid = default (Guid))
        {
            Content = content;
            Guid = guid;
        }

        public override string ToString()
        {
            return Content.ToString();
        }
    }
}
