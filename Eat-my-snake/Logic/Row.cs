using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EatMySnake.Core.Logic
{
    class Row
    {
        private Content content;
        private Except except;
        private AndOrState andOrState;

        public Row()
        {
            this.content = Content.Empty;
            this.except = Except.No;
            this.andOrState = AndOrState.And_Grey;
        }

        public Row(Content content)
        {
            this.content = content;
            this.except = Except.No;
            this.andOrState = AndOrState.And_Grey;
        }

        public Row(Content content, Except except, AndOrState andOrState)
        {
            this.content = content;
            this.except = except;
            this.andOrState = andOrState;
        }

        public override int GetHashCode()
        {
            int a = (int)content;
            int b = (int)except;
            return a ^ b;
        }

        public override bool Equals(object obj)
        {
            Row _row = (Row)obj;
            if (except == Except.No)
            {
                return content == _row.content;
            }
            return content != _row.content;
        }
    }
}
