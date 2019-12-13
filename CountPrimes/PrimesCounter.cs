using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CountPrimes
{
    class PrimesCounter
    {
        private const int min = 2;
        private static readonly ConcurrentQueue<int> primes = new ConcurrentQueue<int>();

        /// <summary>
        /// Implement this.
        /// Method will block execution until calculation is done.
        /// </summary>
        /// <param name="max">Search for primes up to this number</param>
        /// <param name="threads">Number of threads to use</param>
        /// <returns>Number of primes</returns>
        public int CountUpTo(int max, int threads)
        {
            var threadsList = new Thread[threads];
            var range = max / threads;
            var start = 2;
            for (var i = 0; i < threads - 1; i++)
            {
                var startl = start;
                threadsList[i] = new Thread(() => GeneratePrimes(startl, range));
                start += range;
                threadsList[i].Start();
            }
            threadsList[threads - 1] = new Thread(() => GeneratePrimes(start, max - start));
            threadsList[threads - 1].Start();

            for (var i = 0; i < threads; i++)
                threadsList[i].Join();


            return primes.Count;
        }

        private static void GeneratePrimes(int start, int range)
        {
            var isPrime = true;
            var end = start + range;
            for (var i = start; i < end; i++)
            {
                for (var j = min; j < Math.Sqrt(end); j++)
                {
                    if (i != j && i % j == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }
                if (isPrime)
                {
                    primes.Enqueue(i);
                }
                isPrime = true;
            }
        }
    }
}
