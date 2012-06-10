using System;
using System.Linq;
using SnakeBattleNet.Core.Battlefield;
using SnakeBattleNet.Core.Common;

namespace SnakeBattleNet.Core.Snake
{
    public class ModuleRow
    {
        public ModuleRowContent ModuleRowContent { get; private set; }
        public Exclude Exclude { get; private set; }
        public AOColor AoColor { get; private set; }
        public string Id { get; private set; }

        public ModuleRow(AOColor aoColor) : this(ModuleRowContent.Undefined, Exclude.No, aoColor, null) { }

        public ModuleRow(ModuleRowContent moduleRowContent, Exclude exclude, AOColor aoColor) : this(moduleRowContent, exclude, aoColor, null) { }

        public ModuleRow(ModuleRowContent moduleRowContent, Exclude exclude, AOColor aoColor, string id)
        {
            this.ModuleRowContent = moduleRowContent;
            Exclude = exclude;
            AoColor = aoColor;
            Id = id;
        }

        public override string ToString()
        {
            return this.ModuleRowContent.ToString();
        }

        public override int GetHashCode()
        {
            int a = (int)this.ModuleRowContent;
            if (this.Exclude == Exclude.No)
                return a;

            int hs = Enum.GetValues(typeof(ModuleRowContent)).Cast<int>().Sum();
            return hs - a;
        }

        public override bool Equals(object o)
        {
            if (o == null)
            {
                return this.ModuleRowContent == ModuleRowContent.Undefined;
            }

            if (o is FieldRow)
            {
                var fieldRow = o as FieldRow;

                switch (this.ModuleRowContent)
                {
                    case ModuleRowContent.Empty:
                        return FieldEquals(fieldRow, FieldRowContent.Empty);
                    case ModuleRowContent.Wall:
                        return FieldEquals(fieldRow, FieldRowContent.Wall);
                    case ModuleRowContent.OwnHead:
                        {
                            if ((fieldRow.FieldRowContent == FieldRowContent.Head)
                                && (this.Id == fieldRow.Id))
                            {
                                return true;
                            }
                            return false;
                        }
                    case ModuleRowContent.OwnBody:
                        return OwnEquals(fieldRow, FieldRowContent.Body);
                    case ModuleRowContent.OwnTail:
                        return OwnEquals(fieldRow, FieldRowContent.Tail);
                    case ModuleRowContent.EnemyHead:
                        return EnemyEquals(fieldRow, FieldRowContent.Head);
                    case ModuleRowContent.EnemyBody:
                        return EnemyEquals(fieldRow, FieldRowContent.Body);
                    case ModuleRowContent.EnemyTail:
                        return EnemyEquals(fieldRow, FieldRowContent.Tail);
                    case ModuleRowContent.Undefined:
                        return true;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (o is ModuleRow)
            {
                var chipRow = o as ModuleRow;
                if (this.Exclude == Exclude.No)
                {
                    return this.ModuleRowContent == chipRow.ModuleRowContent;
                }
                return this.ModuleRowContent != chipRow.ModuleRowContent;
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
                && (this.Id == fieldRow.Id))
            {
                return true;
            }
            if ((this.Exclude == Exclude.Yes)
                && ((fieldRow.FieldRowContent != fieldRowContent)
                    || (this.Id != fieldRow.Id)))
            {
                return true;
            }
            return false;
        }

        private bool EnemyEquals(FieldRow fieldRow, FieldRowContent fieldRowContent)
        {
            if ((this.Exclude == Exclude.No)
                && (fieldRow.FieldRowContent == fieldRowContent)
                && (this.Id != fieldRow.Id))
            {
                return true;
            }
            if ((this.Exclude == Exclude.Yes)
                && ((fieldRow.FieldRowContent != fieldRowContent)
                    || (this.Id == fieldRow.Id)))
            {
                return true;
            }
            return false;
        }
    }
}
