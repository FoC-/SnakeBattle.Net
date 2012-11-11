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
        public static string F(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        ///<summary>
        /// Indicates whether String object is <c>null</c> or an Empty string.
        ///</summary>
        public static bool IsNullOrEmpty(this string source)
        {
            return string.IsNullOrEmpty(source);
        }
    }
}
