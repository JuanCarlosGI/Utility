using System;
using System.Collections.Generic;

namespace Utility
{
    /// <summary>
    /// Class that implements the IComparer interface through a Comparison object.
    /// </summary>
    /// <typeparam name="T">Type of objects it compares.</typeparam>
    public class FuncComparer<T> : IComparer<T>
    {
        private readonly Comparison<T> _comparison;

        /// <summary>
        /// Initializes a new instance of the <see cref="FuncComparer{T}"/> class.
        /// </summary>
        /// <param name="comparison">Comparison to be used.</param>
        public FuncComparer(Comparison<T> comparison)
        {
            _comparison = comparison;
        }

        /// <summary>
        /// Compares two objects of type <see cref="T"/>.
        /// </summary>
        /// <param name="x">First object</param>
        /// <param name="y">Second object</param>
        /// <returns></returns>
        public int Compare(T x, T y)
        {
            return _comparison(x, y);
        }
    }
}
