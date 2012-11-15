using SnakeBattleNet.Core.Common;

namespace SnakeBattleNet.Core.Battlefield
{
    public class FieldRow
    {
        public FieldRowContent FieldRowContent { get; private set; }
        public string Id { get; private set; }

        public FieldRow(FieldRowContent fieldRowContent, string id)
        {
            FieldRowContent = fieldRowContent;
            Id = id;
        }

        public override string ToString()
        {
            return FieldRowContent.ToString();
        }
    }
}
