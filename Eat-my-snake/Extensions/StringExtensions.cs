using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace EatMySnake.Core.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Does the same as string.Format(), but is more redable.
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
