using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Utility.Algorithms.PrimeNumbers
{
    /// <summary>
    /// Class that offers an implementation of the Sieve of Eratosthenes.
    /// </summary>
    public static class Sieve
    {
        /// <summary>
        /// Generates all prime numbers up to an upper limit value.
        /// </summary>
        /// <param name="upperBound">The maximum value a prime can take.</param>
        /// <returns>An array containing all the found prime numbers.</returns>
        public static int[] GeneratePrimes(int upperBound)
        {
            var r = new List<int>();
            var primes = new BitArray(upperBound, true)
            {
                [0] = false,
                [1] = false
            };

            for (var i = 0; i < upperBound; i++)
            {
                if (!primes[i]) continue;
                r.Add(i);
                for (var j = i * i; j < upperBound; j += i)
                    primes[j] = false;
            }

            return r.ToArray();
        }
    }
}
