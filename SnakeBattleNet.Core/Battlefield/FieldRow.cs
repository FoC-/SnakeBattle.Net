﻿using System;
using SnakeBattleNet.Core.Common;

namespace SnakeBattleNet.Core.Battlefield
{
    public class FieldRow
    {
        public Content Content { get; private set; }
        public Guid Guid { get; private set; }

        public FieldRow() : this(Content.Empty) { }

        public FieldRow(Content content, Guid guid = default (Guid))
        {
            Content = content;
            Guid = guid;
        }

        public override string ToString()
        {
            return Content.ToString();
        }
    }
}