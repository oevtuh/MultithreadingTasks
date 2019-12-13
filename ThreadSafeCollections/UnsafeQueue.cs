using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadSafeCollections
{
    class UnsafeQueue<T> : SimpleQueue<T>
    {
        private Queue<T> queue = new Queue<T>();

        public override T Dequeue()
        {
            return queue.Dequeue();
        }

        public override void Enqueue(T item)
        {
            queue.Enqueue(item);
        }

        public override int Count
        {
            get { return queue.Count; }
        }
    }
}
