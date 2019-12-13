using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadSafeCollections
{
    class SafeQueue<T> : SimpleQueue<T>
    {
        private readonly Object lockObject = new Object();
        private Queue<T> queue = new Queue<T>();

        public override T Dequeue()
        {
            lock (lockObject)
            {
                return queue.Dequeue();
            }
        }

        public override void Enqueue(T item)
        {
            lock (lockObject)
            {
                queue.Enqueue(item);
            }
        }

        public override int Count {
            get { return queue.Count; }
        }
    }
}
