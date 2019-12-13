using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CountPrimes
{
    class Program
    {
        static void Main(string[] args)
        {
            PrimesCounter counter = new PrimesCounter();            
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //Sample use
            int primes = counter.CountUpTo(100, 2);

            //int primes = counter.getAllPrimes(100000);
            sw.Stop();
            Console.WriteLine("Primes: {0} in {1} msec", primes, sw.ElapsedMilliseconds);

            // Run for counter.CountUpTo(1000000, 1); =  656 ms
            // Run for counter.CountUpTo(1000000, 2); =  342 ms
            // Run for counter.CountUpTo(1000000, 3); =  251 ms
            // Run for counter.CountUpTo(1000000, 4); =  209 ms
            // Run for counter.CountUpTo(1000000, 5); =  176 ms
            // Run for counter.CountUpTo(1000000, 6); =  144 ms
            // Run for counter.CountUpTo(1000000, 7); =  130 ms
            // Run for counter.CountUpTo(1000000, 8); =  172 ms

            // I think not need to set more then 4 threads - because my PC has Number Of Cores:4

            int coreCount = 0;
            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                coreCount += int.Parse(item["NumberOfCores"].ToString());
            }
            Console.WriteLine("Number Of Cores: {0}", coreCount);
        }
    }
}
