using System;
using System.Linq;
using SnakeBattleNet.Core.Battlefield;
using SnakeBattleNet.Core.Common;

namespace SnakeBattleNet.Core.Snake
{
    public class ChipRow
    {
        public ChipRowContent ChipRowContent { get; private set; }
        public Guid Guid { get; private set; }
        public Except Except { get; private set; }
        public AndOrState AndOrState { get; private set; }

        public ChipRow() : this(ChipRowContent.Undefined) { }

        public ChipRow(ChipRowContent chipRowContent, Except except = Except.No, Guid guid = default (Guid)) : this(chipRowContent, except, guid, AndOrState.AndGrey) { }

        public ChipRow(ChipRowContent chipRowContent, Except except, Guid guid, AndOrState andOrState)
        {
            this.ChipRowContent = chipRowContent;
            Guid = guid;
            Except = except;
            AndOrState = andOrState;
        }

        public override int GetHashCode()
        {
            int a = (int)this.ChipRowContent;
            if (Except == Except.No)
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
                if (Except == Except.No)
                {
                    return this.ChipRowContent == chipRow.ChipRowContent;
                }
                return this.ChipRowContent != chipRow.ChipRowContent;
            }

            throw new ArgumentOutOfRangeException();
        }

        private bool FieldEquals(FieldRow fieldRow, FieldRowContent fieldRowContent)
        {
            if ((this.Except == Except.No) && (fieldRow.FieldRowContent == fieldRowContent))
            {
                return true;
            }
            if ((this.Except == Except.Yes) && (fieldRow.FieldRowContent != fieldRowContent))
            {
                return true;
            }
            return false;
        }

        private bool OwnEquals(FieldRow fieldRow, FieldRowContent fieldRowContent)
        {
            if ((this.Except == Except.No)
                && (fieldRow.FieldRowContent == fieldRowContent)
                && (this.Guid == fieldRow.Guid))
            {
                return true;
            }
            if ((this.Except == Except.Yes)
                && ((fieldRow.FieldRowContent != fieldRowContent)
                    || (this.Guid != fieldRow.Guid)))
            {
                return true;
            }
            return false;
        }

        private bool EnemyEquals(FieldRow fieldRow, FieldRowContent fieldRowContent)
        {
            if ((Except == Except.No)
                && (fieldRow.FieldRowContent == fieldRowContent)
                && (this.Guid != fieldRow.Guid))
            {
                return true;
            }
            if ((this.Except == Except.Yes)
                && ((fieldRow.FieldRowContent != fieldRowContent)
                    || (this.Guid == fieldRow.Guid)))
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return this.ChipRowContent.ToString();
        }
    }
}
