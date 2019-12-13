using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace ThreadSafeCollections
{
    public class Tests
    {
        private void AddManyItems(SimpleQueue<string> queue)
        {
            for (int x = 0; x < 1000; x++)
            {
                var newName = string.Format("name {0}", x);
                queue.Enqueue(newName);
            }
        }
        private void AddManyItemsToDictionary(Dictionary<string, string> dictionary, int from)
        {
            for (int x = from; x < from + 1000; x++)
            {
                var newName = string.Format("name {0}", x);
                dictionary.Add(x.ToString(), newName);
            }
        }
        private void AddManyItemsToDictionary(SafeDictionary<string, string> dictionary, int from)
        {
            for (int x = from; x < from + 1000; x++)
            {
                var newName = string.Format("name {0}", x);
                dictionary.Set(x.ToString(), newName);
            }
        }
        public void CheckCountForSafeCollection(SimpleQueue<string> queue)
        {
            var t1 = new Thread(() => { AddManyItems(queue);});
            var t2 = new Thread(() => { AddManyItems(queue);});

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Console.WriteLine("Count is {0}", queue.Count);
        }
        public void TestBlockinQueue (BlockingQueue<int> queue)
        {
           var list = new List<int>(){1,4,7,47,85,658,74};

            new Thread(() => { WriteBlockingQueue(list, queue); }).Start();
            new Thread(() => { ReadBlockingQueue(queue); }).Start();
        }

        public void TestBlockinQueueInvalid(BlockingQueue<int> queue)
        {
            var list = new List<int>() { 1, 4, 7, 47, 85, 658, 74 };

            new Thread(() => { WriteBlockingQueue(list, queue); }).Start();
            new Thread(() => { ReadBlockingQueue(queue); }).Start();
            new Thread(() => { ReadBlockingQueue(queue); }).Start();
        }
        private void ReadBlockingQueue(BlockingQueue<int> queue)
        {
            Console.WriteLine("ReadBlockingQueue started");
            while (true)
            {
                Console.WriteLine("trying to read");
                Console.WriteLine(queue.Dequeue());
            }
        }
        private void WriteBlockingQueue(List<int> list, BlockingQueue<int> queue)
        {
            Console.WriteLine("WriteBlockingQueue started");
            foreach (var i in list)
            {
                Console.WriteLine("inserting i "+ i);
                queue.Enqueue(i);
                Thread.Sleep(2000);
            }
        }
        public void CheckUnsafeDictionary(Dictionary<string, string> dictionary)
        {
            var t1 = new Thread(() => { AddManyItemsToDictionary(dictionary, 0); });
            var t2 = new Thread(() => { AddManyItemsToDictionary(dictionary, 1001); });

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Console.WriteLine("Count unsafe dictionary is {0}", dictionary.Count);
        }
        public void CheckSafeDictionary(SafeDictionary<string, string> dictionary)
        {
            var t1 = new Thread(() => { AddManyItemsToDictionary(dictionary, 0); });
            var t2 = new Thread(() => { AddManyItemsToDictionary(dictionary, 1001); });

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Console.WriteLine("Count safe dictionary is {0}", dictionary.GetCount());
        }
    }
}
