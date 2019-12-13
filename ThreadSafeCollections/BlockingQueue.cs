using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadSafeCollections
{
    public class BlockingQueue<T> : SimpleQueue<T>
    {
        private Queue<T> queue = new Queue<T>();

        AutoResetEvent waitHandler = new AutoResetEvent(false);
        public override int Count
        {
            get { return queue.Count; }
        }

        public override T Dequeue()
        {
            if (queue.Count == 0)
            {
                waitHandler.WaitOne();
            }

            var element = queue.Dequeue();
            return element;
        }

        public override void Enqueue(T item)
        {
            queue.Enqueue(item);
            waitHandler.Set();
        }
    }
}
