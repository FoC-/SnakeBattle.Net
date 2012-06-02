using System;
using System.Linq;
using SnakeBattleNet.Core.Common;

namespace SnakeBattleNet.Core.Snake
{
    public class ChipRow
    {
        public Content Content { get; private set; }
        public Guid Guid { get; private set; }
        public Except Except { get; private set; }
        public AndOrState AndOrState { get; private set; }

        public ChipRow() : this(Content.Empty) { }

        public ChipRow(Content content, Guid guid = default (Guid)) : this(content, guid, Except.No, AndOrState.AndGrey) { }

        public ChipRow(Content content, Guid guid, Except except, AndOrState andOrState)
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
            ChipRow chipRow = (ChipRow)obj;
            if (Except == Except.No)
            {
                return Content == chipRow.Content;
            }
            return Content != chipRow.Content;
        }

        public override string ToString()
        {
            return Content.ToString();
        }
    }
}
