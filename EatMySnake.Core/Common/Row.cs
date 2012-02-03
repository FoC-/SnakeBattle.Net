using System;
using System.Linq;

namespace EatMySnake.Core.Common
{
    public class Row
    {
        public Content Content { get; private set; }
        public Guid Guid { get; private set; }
        public Except Except { get; private set; }
        public AndOrState AndOrState { get; private set; }

        public Row() : this(Content.Empty) { }

        public Row(Content content, Guid guid = default (Guid)) : this(content, guid, Except.No, AndOrState.AndGrey) { }

        public Row(Content content, Guid guid, Except except, AndOrState andOrState)
        {
            Content = content;
            Guid = guid;
            Except = except;
            AndOrState = andOrState;
        }

        public override int GetHashCode()
        {
            int a = (int)Content;
            if (Except == Except.No)
                return a;

            int hs = Enum.GetValues(typeof(Content)).Cast<int>().Sum();
            return hs - a;
        }

        public override bool Equals(object obj)
        {
            Row row = (Row)obj;
            if (Except == Except.No)
            {
                return Content == row.Content;
            }
            return Content != row.Content;
        }

        public override string ToString()
        {
            return Content.ToString();
        }
    }
}
