using System;
using SnakeBattleNet.Core.Common;

namespace SnakeBattleNet.Core.Battlefield
{
    public class FieldRow
    {
        public FieldRowContent FieldRowContent { get; private set; }
        public string Id { get; private set; }

        public FieldRow() : this(FieldRowContent.Empty) { }

        public FieldRow(FieldRowContent fieldRowContent, string id = null)
        {
            this.FieldRowContent = fieldRowContent;
            Id = id;
        }

        public override string ToString()
        {
            return this.FieldRowContent.ToString();
        }
    }
}
