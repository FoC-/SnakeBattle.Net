using JetBrains.Annotations;

namespace SnakeBattleNet.Utils.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Does the same as string.Format(), but is more readable.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        [StringFormatMethod("format")]
        public static string F(this string format, params object[] args)
        {
            return string.Format(format, args);
        }
    }
}
