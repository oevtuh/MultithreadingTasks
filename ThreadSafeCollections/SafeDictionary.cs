using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreadSafeCollections
{
    public class SafeDictionary<TKey, TValue>
    {
        private Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
        private readonly Object lockObject = new Object();
        public TValue Get(TKey key)
        {
            TValue value;
            return dict.TryGetValue(key, out value)? value: default(TValue);
        }

        public void Set(TKey key, TValue value)
        {
            lock (lockObject)
            {
                dict.Add(key, value);
            }
        }

        public int GetCount()
        {
            lock (lockObject)
            {
                return dict.Count;
            }
        }
    }
}
