using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Deadlocks
{
    class Program
    {
        static object a = new object();
        static object b = new object();

        static void ThreadAB()
        {
            while (true)
            {
                Console.WriteLine("1 works");
                while (true)
                {
                    try
                    {
                        bool entered = Monitor.TryEnter(a, TimeSpan.FromMilliseconds(100));
                        if (!entered) continue;
                        entered = Monitor.TryEnter(b, TimeSpan.FromMilliseconds(100));
                        if (!entered)
                        {
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("Reached");
                        }
                        
                        break;
                    }
                    finally
                    {
                        if (Monitor.IsEntered(a)) Monitor.Exit(a);
                        if (Monitor.IsEntered(b)) Monitor.Exit(b);
                        Thread.Sleep(0);
                    }
                }
                Thread.Sleep(0);
            }
        }

        static void ThreadBA()
        {
            while (true)
            {
                Console.WriteLine("2 works");
                lock (b)
                {
                    Thread.Sleep(0);
                    lock (a)
                    {
                        Thread.Sleep(0);
                    }
                }
                Thread.Sleep(0);
            }
        }

        static void Main(string[] args)
        {
            Thread ab = new Thread(ThreadAB);
            Thread ba = new Thread(ThreadBA);
            ab.Start();
            ba.Start();
            Thread.Sleep(1000);
            while (true)
            {
                while (ab.ThreadState != ThreadState.WaitSleepJoin && ba.ThreadState != ThreadState.WaitSleepJoin)
                {
                    Thread.Sleep(0);
                }
                Console.WriteLine("Deadlock!");
                Thread.Sleep(1000);
            }
        }
    }
}
