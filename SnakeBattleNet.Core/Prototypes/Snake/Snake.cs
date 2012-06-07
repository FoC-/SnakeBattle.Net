using System;
using System.Collections.Generic;

namespace SnakeBattleNet.Core.Prototypes
{
    /// <summary>
    /// Represents a snake from user perspective.
    /// </summary>
    public class Snake
    {
        public int Id { get; internal set; }
        public List<MindChip> MindChips { get; set; }
        public Passport Passport { get; set; }
        public Version Version { get; set; }
    }
}