using System;
using SnakeBattleNet.Core.Common;

namespace SnakeBattleNet.Core.Battlefield
{
    public class FieldRow
    {
        public FieldRowContent FieldRowContent { get; private set; }
        public Guid Guid { get; private set; }

        public FieldRow() : this(FieldRowContent.Empty) { }

        public FieldRow(FieldRowContent fieldRowContent, Guid guid = default (Guid))
        {
            this.FieldRowContent = fieldRowContent;
            Guid = guid;
        }

        public override string ToString()
        {
            return this.FieldRowContent.ToString();
        }
    }
}
