using System;

namespace SnakeBattleNet.Web.Utils
{
    internal static class StringExtensions
    {
        public static string F(this string str, params object[] os)
        {
            return String.Format(str, os);
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return String.IsNullOrWhiteSpace(str);
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return String.IsNullOrEmpty(str);
        }

        public static bool IsNotNullOrEmpty(this string str)
        {
            return !String.IsNullOrEmpty(str);
        }
    }
}