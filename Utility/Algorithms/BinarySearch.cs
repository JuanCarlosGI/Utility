using System;
using System.Collections.Generic;

namespace Utility.Algorithms
{
    /// <summary>
    /// Static class that provides algorithms for binary searches.
    /// </summary>
    public static class BinarySearch
    {
        /// <summary>
        /// Searches for an element in a collection.
        /// </summary>
        /// <typeparam name="T">Type of elements in collection. Must implement "Equals".</typeparam>
        /// <param name="collection">The collection of elements in which it will search.</param>
        /// <param name="element">The element to be searched for.</param>
        /// <param name="comparer">The comparison to be used.</param>
        /// <returns>The index of the element, or -1 if not found.</returns>
        public static long IndexOf<T>(IReadOnlyList<T> collection, T element, Comparison<T> comparer)
        {
            var start = 0;
            var end = collection.Count - 1;

            while (start < end)
            {
                var mid = start + (end - start) / 2;
                if (collection[mid].Equals(element)) return mid;
                if (comparer(collection[mid], element) < 0)
                    start = mid + 1;
                else
                    end = mid;
            }

            return -1;
        }

        /// <summary>
        /// Searches for the last element that meets a specific criteria.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection</typeparam>
        /// <param name="collection">The collection of elements in which it will search.</param>
        /// <param name="predicate">The criteria to be met.</param>
        /// <returns>The index of the last element that meets the criteria, or -1 if none do.</returns>
        public static long IndexOfLast<T>(IReadOnlyList<T> collection, Predicate<T> predicate)
        {
            if (collection.Count == 0 || !predicate(collection[0])) return -1;

            var start = 0;
            var end = collection.Count - 1;
            while (start < end)
            {
                var mid = start + (end - start) / 2 + (start%2 ^ end%2);
                if (predicate(collection[mid]))
                    start = mid;
                else
                    end = mid - 1;
            }

            return start;
        }

        /// <summary>
        /// Searches for the last number in a range that meets a specific criteria.
        /// </summary>
        /// <param name="lowerBound">Lower bound of the search domain.</param>
        /// <param name="upperBound">Upper bound of the search domain.</param>
        /// <param name="predicate">The criteria to be met.</param>
        /// <returns>The last element that meets the criteria, or long.MinValue if none do.</returns>
        public static long LastInRange(long lowerBound, long upperBound, Predicate<long> predicate)
        {
            if (!predicate(lowerBound)) return long.MinValue;

            var start = lowerBound;
            var end = upperBound;
            while (start < end)
            {
                var mid = start + (end - start) / 2 + (start % 2 ^ end % 2);
                if (predicate(mid))
                    start = mid;
                else
                    end = mid - 1;
            }

            return start;
        }
    }
}
