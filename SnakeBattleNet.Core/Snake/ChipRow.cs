using System;
using System.Linq;
using SnakeBattleNet.Core.Battlefield;
using SnakeBattleNet.Core.Common;

namespace SnakeBattleNet.Core.Snake
{
    public class ChipRow
    {
        public ChipRowContent ChipRowContent { get; private set; }
        public Exclude Exclude { get; private set; }
        public AOColor AoColor { get; private set; }
        public Guid Guid { get; private set; }

        public ChipRow(AOColor aoColor) : this(ChipRowContent.Undefined, Exclude.No, aoColor, default(Guid)) { }

        public ChipRow(ChipRowContent chipRowContent, Exclude exclude, AOColor aoColor) : this(chipRowContent, exclude, aoColor, default(Guid)) { }

        public ChipRow(ChipRowContent chipRowContent, Exclude exclude, AOColor aoColor, Guid guid)
        {
            ChipRowContent = chipRowContent;
            Exclude = exclude;
            AoColor = aoColor;
            Guid = guid;
        }

        public override string ToString()
        {
            return ChipRowContent.ToString();
        }

        public override int GetHashCode()
        {
            int a = (int)this.ChipRowContent;
            if (this.Exclude == Exclude.No)
                return a;

            int hs = Enum.GetValues(typeof(ChipRowContent)).Cast<int>().Sum();
            return hs - a;
        }

        public override bool Equals(object o)
        {
            if (o == null)
            {
                return this.ChipRowContent == ChipRowContent.Undefined;
            }

            if (o is FieldRow)
            {
                var fieldRow = o as FieldRow;

                switch (this.ChipRowContent)
                {
                    case ChipRowContent.Empty:
                        return FieldEquals(fieldRow, FieldRowContent.Empty);
                    case ChipRowContent.Wall:
                        return FieldEquals(fieldRow, FieldRowContent.Wall);
                    case ChipRowContent.OwnHead:
                        {
                            if ((fieldRow.FieldRowContent == FieldRowContent.Head)
                                && (this.Guid == fieldRow.Guid))
                            {
                                return true;
                            }
                            return false;
                        }
                    case ChipRowContent.OwnBody:
                        return OwnEquals(fieldRow, FieldRowContent.Body);
                    case ChipRowContent.OwnTail:
                        return OwnEquals(fieldRow, FieldRowContent.Tail);
                    case ChipRowContent.EnemyHead:
                        return EnemyEquals(fieldRow, FieldRowContent.Head);
                    case ChipRowContent.EnemyBody:
                        return EnemyEquals(fieldRow, FieldRowContent.Body);
                    case ChipRowContent.EnemyTail:
                        return EnemyEquals(fieldRow, FieldRowContent.Tail);
                    case ChipRowContent.Undefined:
                        return true;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (o is ChipRow)
            {
                var chipRow = o as ChipRow;
                if (this.Exclude == Exclude.No)
                {
                    return this.ChipRowContent == chipRow.ChipRowContent;
                }
                return this.ChipRowContent != chipRow.ChipRowContent;
            }

            throw new ArgumentOutOfRangeException();
        }

        private bool FieldEquals(FieldRow fieldRow, FieldRowContent fieldRowContent)
        {
            if ((this.Exclude == Exclude.No) && (fieldRow.FieldRowContent == fieldRowContent))
            {
                return true;
            }
            if ((this.Exclude == Exclude.Yes) && (fieldRow.FieldRowContent != fieldRowContent))
            {
                return true;
            }
            return false;
        }

        private bool OwnEquals(FieldRow fieldRow, FieldRowContent fieldRowContent)
        {
            if ((this.Exclude == Exclude.No)
                && (fieldRow.FieldRowContent == fieldRowContent)
                && (this.Guid == fieldRow.Guid))
            {
                return true;
            }
            if ((this.Exclude == Exclude.Yes)
                && ((fieldRow.FieldRowContent != fieldRowContent)
                    || (this.Guid != fieldRow.Guid)))
            {
                return true;
            }
            return false;
        }

        private bool EnemyEquals(FieldRow fieldRow, FieldRowContent fieldRowContent)
        {
            if ((this.Exclude == Exclude.No)
                && (fieldRow.FieldRowContent == fieldRowContent)
                && (this.Guid != fieldRow.Guid))
            {
                return true;
            }
            if ((this.Exclude == Exclude.Yes)
                && ((fieldRow.FieldRowContent != fieldRowContent)
                    || (this.Guid == fieldRow.Guid)))
            {
                return true;
            }
            return false;
        }
    }
}
