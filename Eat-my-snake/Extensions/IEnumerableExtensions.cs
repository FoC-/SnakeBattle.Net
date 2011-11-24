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
        private static readonly Random random =  new Random();

        /// <summary>
        /// Shuffles <see cref="IEnumerable{T}"/> in random order.
        /// </summary>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
        {
            //this could be implemented in one line: return enumerable.OrderBy(_ => r.Next());
            //but according to this http://stackoverflow.com/questions/1287567/c-is-using-random-and-orderby-a-good-shuffle-algorithm 
            //I decided to use Fisher-Yates shuffle

            T[] elements = enumerable.ToArray();
            // Note i > 0 to avoid final pointless iteration
            for (int i = elements.Length - 1; i > 0; i--)
            {
                // Swap element "i" with a random earlier element it (or itself)
                int swapIndex = random.Next(i + 1);
                yield return elements[swapIndex];
                elements[swapIndex] = elements[i];
                // we don't actually perform the swap, we can forget about the
                // swapped element because we already returned it.
            }

            // there is one item remaining that was not returned - we return it now
            yield return elements[0]; 
        }
    }
}