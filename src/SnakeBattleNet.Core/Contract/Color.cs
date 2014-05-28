using System.Collections.Generic;

namespace SnakeBattleNet.Core.Contract
{
    public class Color
    {
        public string Name { get; private set; }
        public bool IsAnd { get; private set; }

        public static Dictionary<string, Color> All = new Dictionary<string, Color>
        {
            {"Blue", new Color { Name = "Blue", IsAnd = false }},
            {"Green",new Color { Name = "Green", IsAnd = false }},
            {"Grey", new Color { Name = "Grey", IsAnd = true }},
            {"Red",  new Color { Name = "Red", IsAnd = true }},
            {"Black",new Color { Name = "Black", IsAnd = true }},
        };

        public static Color Blue()
        {
            return All["Blue"];
        }

        public static Color Green()
        {
            return All["Green"];
        }

        public static Color Grey()
        {
            return All["Grey"];
        }

        public static Color Red()
        {
            return All["Red"];
        }

        public static Color Black()
        {
            return All["Black"];
        }
    }
}
