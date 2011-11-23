using System;
using System.Collections.Generic;
using System.Linq;

namespace EatMySnake.Core.Extensions
{
    /// <summary>
    /// Contains extension methods for <see cref="IEnumerable{T}"/>
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Shuffles <see cref="IEnumerable{T}"/> in random order.
        /// </summary>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
        {

            var r = new Random();
            return enumerable.OrderBy(x => r.Next()).ToList();
            throw new NotImplementedException();
        }
    }
}