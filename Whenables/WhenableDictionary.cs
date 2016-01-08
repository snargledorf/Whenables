using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Whenables.Core;

namespace Whenables
{
    public class WhenableDictionary<TKey, TValue> : IWhenableDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> dict;

        private readonly KeyValueSetterManager<TKey, TValue> addManager = new KeyValueSetterManager<TKey, TValue>();
        private readonly KeyValueSetterManager<TKey, TValue> removeManager = new KeyValueSetterManager<TKey, TValue>();
        private readonly KeyValueSetterManager<TKey, TValue> insertManager = new KeyValueSetterManager<TKey, TValue>();

        public WhenableDictionary()
        {
            dict = new Dictionary<TKey, TValue>();
        }

        public WhenableDictionary(int capacity)
        {
            dict = new Dictionary<TKey, TValue>(capacity);
        }

        public WhenableDictionary(IEqualityComparer<TKey> comparer)
        {
            dict = new Dictionary<TKey, TValue>(comparer);
        }

        public WhenableDictionary(IDictionary<TKey, TValue> dictionary)
        {
            dict = new Dictionary<TKey, TValue>(dictionary);
        }

        public WhenableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            dict = new Dictionary<TKey, TValue>(dictionary, comparer);
        }

        public TValue this[TKey key]
        {
            get { return dict[key]; }
            set
            {
                dict[key] = value;
                insertManager.TrySet(key, value);
            }
        }

        public ICollection<TKey> Keys => dict.Keys;

        public ICollection<TValue> Values => dict.Values;

        public int Count => dict.Count;

        public bool IsReadOnly => dict.IsReadOnly;

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            dict.Add(item);
            addManager.TrySetResult(item);
        }

        public void Clear()
        {
            dict.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) => dict.Contains(item);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            dict.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (!dict.Remove(item))
                return false;

            removeManager.TrySetResult(item);

            return true;
        }

        public bool ContainsKey(TKey key) => dict.ContainsKey(key);

        public void Add(TKey key, TValue value)
        {
            dict.Add(key, value);
            addManager.TrySet(key, value);
        }

        public bool Remove(TKey key)
        {
            if (!ContainsKey(key))
                return false;

            TValue value = this[key];

            if (!dict.Remove(key))
                return false;

            removeManager.TrySet(key, value);

            return true;
        }

        public bool TryGetValue(TKey key, out TValue value) => dict.TryGetValue(key, out value);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => dict.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)dict).GetEnumerator();

        public IKeyValueResultAccessor<TKey, TValue> WhenAdded(Func<TKey, bool> condition)
            => WhenAdded(kvp => condition(kvp.Key));

        public IKeyValueResultAccessor<TKey, TValue> WhenAdded(Func<TValue, bool> condition)
            => WhenAdded(kvp => condition(kvp.Value));

        public IKeyValueResultAccessor<TKey, TValue> WhenAdded(Func<TKey, TValue, bool> condition)
            => WhenAdded(kvp => condition(kvp.Key, kvp.Value));

        public IKeyValueResultAccessor<TKey, TValue> WhenAdded(Func<KeyValuePair<TKey, TValue>, bool> condition)
            => CreateAddInsertCondition(condition, addManager);

        public IKeyValueResultAccessor<TKey, TValue> WhenInserted(Func<TKey, bool> condition)
            => WhenInserted(kvp => condition(kvp.Key));

        public IKeyValueResultAccessor<TKey, TValue> WhenInserted(Func<TValue, bool> condition)
            => WhenInserted(kvp => condition(kvp.Value));

        public IKeyValueResultAccessor<TKey, TValue> WhenInserted(Func<TKey, TValue, bool> condition)
            => WhenInserted(kvp => condition(kvp.Key, kvp.Value));

        public IKeyValueResultAccessor<TKey, TValue> WhenInserted(Func<KeyValuePair<TKey, TValue>, bool> condition)
            => CreateAddInsertCondition(condition, insertManager);

        public IKeyValueResultAccessor<TKey, TValue> WhenRemoved(Func<TKey, bool> condition)
            => WhenRemoved(kvp => condition(kvp.Key));

        public IKeyValueResultAccessor<TKey, TValue> WhenRemoved(Func<TValue, bool> condition)
            => WhenRemoved(kvp => condition(kvp.Value));

        public IKeyValueResultAccessor<TKey, TValue> WhenRemoved(Func<TKey, TValue, bool> condition)
            => WhenRemoved(kvp => condition(kvp.Key, kvp.Value));

        public IKeyValueResultAccessor<TKey, TValue> WhenRemoved(Func<KeyValuePair<TKey, TValue>, bool> condition)
            => CreateCondition(condition, removeManager);

        private IKeyValueResultAccessor<TKey, TValue> CreateAddInsertCondition(Func<KeyValuePair<TKey, TValue>, bool> condition, 
            IResultSetterManager<KeyValuePair<TKey, TValue>> manager)
        {
            var c = new KeyValueCondition<TKey, TValue>(condition);

            // Go through all of the existing items to see if this condition
            // has already been met. Otherwise the caller could potentially
            // wait forever.
            bool conditionNotMet = true;
            foreach (KeyValuePair<TKey, TValue> kvp in dict)
                if (c.TrySetResult(kvp))
                    conditionNotMet = false;

            // The condition was not met by any of the items in the dictionary. 
            // Add the condition to the manager to be monitored.
            if (conditionNotMet)
                manager.Add(c);

            return c;
        }

        private static IKeyValueResultAccessor<TKey, TValue> CreateCondition(Func<KeyValuePair<TKey, TValue>, bool> condition,
            IResultSetterManager<KeyValuePair<TKey, TValue>> manager)
        {
            var c = new KeyValueCondition<TKey, TValue>(condition);
            manager.Add(c);
            return c;
        }
    }
}
