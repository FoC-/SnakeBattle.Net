using System.Collections.Generic;

namespace SnakeBattleNet.Core.Contract
{
    public abstract class Color
    {
        public interface IColor
        {
            bool IsAnd { get; }
        }

        public class And : IColor
        {
            public bool IsAnd { get { return true; } }
        }
        public class Or : IColor
        {
            public bool IsAnd { get { return false; } }
        }

        public class Blue : Or
        {
        }
        public class Green : Or
        {
        }
        public class Grey : And
        {
        }
        public class Red : And
        {
        }
        public class Black : And
        {
        }

        public static IEnumerable<IColor> All = new IColor[] { new Blue(), new Green(), new Grey(), new Red(), new Black() };
    }
}
