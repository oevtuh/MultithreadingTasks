using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadSafeCollections
{
    class Program
    {
        public static void Add1000Items(SafeQueue<string> queue)
        {
            for (int x = 0; x < 1000; x++)
            {
                var newName = string.Format("name {0}", x);
                queue.Enqueue(newName);
            }
        }

        static void Main(string[] args)
        {
            var test = new Tests();
            // 1. Test Unsafe queue
            //var unsafeQueue = new UnsafeQueue<string>();

            //test.CheckCountForSafeCollection(unsafeQueue);

            //// 2. Test Safe queue
            //var queue = new SafeQueue<string>();
            //test.CheckCountForSafeCollection(queue);

            //3. Test unsafe dictionary
            //var unsafeDictionary = new Dictionary<string, string>();
            //test.CheckUnsafeDictionary(unsafeDictionary);

            //4. Test safe dictionary
            //var safeDictionary = new SafeDictionary<string, string>();
            //test.CheckSafeDictionary(safeDictionary);

            //5. Test Blocking queue
            //BlockingQueue<int> blockingQueue = new BlockingQueue<int>();
            //test.TestBlockinQueue(blockingQueue);

            //6. тест который выдаст System.InvalidOperationException: Queue empty.
            BlockingQueue<int> blockingQueue = new BlockingQueue<int>();
            test.TestBlockinQueueInvalid(blockingQueue);


            Console.ReadLine();
        }
    }
}
