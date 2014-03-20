using System.Collections.Generic;

namespace SnakeBattleNet.Core.Contract
{
    public class Color
    {
        public bool IsAnd { get; set; }
        public string Name { get; set; }

        public static IEnumerable<Color> GetAll()
        {
            return new[]
            {
                Blue(),
                Green(),
                Grey(),
                Red(),
                Black(),
            };
        }

        public static Color Blue()
        {
            return new Color { Name = "Blue" };
        }

        public static Color Green()
        {
            return new Color { Name = "Green" };
        }

        public static Color Grey()
        {
            return new Color { Name = "Grey", IsAnd = true };
        }

        public static Color Red()
        {
            return new Color { Name = "Red", IsAnd = true };
        }

        public static Color Black()
        {
            return new Color { Name = "Black", IsAnd = true };
        }
    }
}
