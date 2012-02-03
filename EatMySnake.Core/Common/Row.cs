using System;
using System.Linq;

namespace EatMySnake.Core.Common
{
    public class Row
    {
        public Content Content { get; set; }
        private readonly Except except;
        private AndOrState andOrState;

        public Row() : this(Content.Empty) { }

        public Row(Content content) : this(content, Except.No, AndOrState.AndGrey) { }

        public Row(Content content, Except except, AndOrState andOrState)
        {
            this.Content = content;
            this.except = except;
            this.andOrState = andOrState;
        }

        public override int GetHashCode()
        {
            int a = (int)Content;
            if (except == Except.No)
                return a;

            int hs = Enum.GetValues(typeof(Content)).Cast<int>().Sum();
            return hs - a;
        }

        public override bool Equals(object obj)
        {
            Row row = (Row)obj;
            if (except == Except.No)
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
