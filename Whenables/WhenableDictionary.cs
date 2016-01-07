using System;
using System.Collections;
using System.Collections.Generic;

namespace Whenables
{
    public class WhenableDictionary<TKey, TValue> : IWhenableDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> dict;

        private readonly DictionaryConditionManager<TKey, TValue> addingDictionaryConditions = new DictionaryConditionManager<TKey, TValue>();
        private readonly DictionaryConditionManager<TKey, TValue> removingDictionaryConditions = new DictionaryConditionManager<TKey, TValue>();
        private readonly DictionaryConditionManager<TKey, TValue> insertDictionaryConditions = new DictionaryConditionManager<TKey, TValue>();

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
                insertDictionaryConditions.SetKeyAndValueOnConditions(key, value);
            }
        }

        public ICollection<TKey> Keys => dict.Keys;

        public ICollection<TValue> Values => dict.Values;

        public int Count => dict.Count;

        public bool IsReadOnly => dict.IsReadOnly;

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            dict.Add(item);
            addingDictionaryConditions.SetKeyAndValueOnConditions(item.Key, item.Value);
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

            removingDictionaryConditions.SetKeyAndValueOnConditions(item.Key, item.Value);

            return true;
        }

        public bool ContainsKey(TKey key) => dict.ContainsKey(key);

        public void Add(TKey key, TValue value)
        {
            dict.Add(key, value);
            addingDictionaryConditions.SetKeyAndValueOnConditions(key, value);
        }

        public bool Remove(TKey key)
        {
            if (!ContainsKey(key))
                return false;

            TValue value = this[key];

            if (!dict.Remove(key))
                return false;

            removingDictionaryConditions.SetKeyAndValueOnConditions(key, value);

            return true;
        }

        public bool TryGetValue(TKey key, out TValue value) => dict.TryGetValue(key, out value);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => dict.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)dict).GetEnumerator();

        public IDictionaryCondition<TKey, TValue> WhenAdded(Func<TKey, bool> condition)
            => CreateCondition((key, value) => condition(key), addingDictionaryConditions);

        public IDictionaryCondition<TKey, TValue> WhenAdded(Func<TValue, bool> condition)
            => CreateCondition((key, value) => condition(value), addingDictionaryConditions);

        public IDictionaryCondition<TKey, TValue> WhenAdded(Func<TKey, TValue, bool> condition)
            => CreateCondition(condition, addingDictionaryConditions);

        public IDictionaryCondition<TKey, TValue> WhenAdded(Func<KeyValuePair<TKey, TValue>, bool> condition)
            => CreateCondition(condition, addingDictionaryConditions);

        public IDictionaryCondition<TKey, TValue> WhenInserted(Func<TKey, bool> condition)
            => CreateCondition((key, value) => condition(key), insertDictionaryConditions);

        public IDictionaryCondition<TKey, TValue> WhenInserted(Func<TValue, bool> condition)
            => CreateCondition((key, value) => condition(value), insertDictionaryConditions);

        public IDictionaryCondition<TKey, TValue> WhenInserted(Func<TKey, TValue, bool> condition)
            => CreateCondition(condition, insertDictionaryConditions);

        public IDictionaryCondition<TKey, TValue> WhenInserted(Func<KeyValuePair<TKey, TValue>, bool> condition)
            => CreateCondition(condition, insertDictionaryConditions);

        public IDictionaryCondition<TKey, TValue> WhenRemoved(Func<TKey, bool> condition)
            => CreateCondition((key, value) => condition(key), removingDictionaryConditions);

        public IDictionaryCondition<TKey, TValue> WhenRemoved(Func<TValue, bool> condition)
            => CreateCondition((key, value) => condition(value), removingDictionaryConditions);

        public IDictionaryCondition<TKey, TValue> WhenRemoved(Func<TKey, TValue, bool> condition)
            => CreateCondition(condition, removingDictionaryConditions);

        public IDictionaryCondition<TKey, TValue> WhenRemoved(Func<KeyValuePair<TKey, TValue>, bool> condition)
            => CreateCondition(condition, removingDictionaryConditions);

        private static IDictionaryCondition<TKey, TValue> CreateCondition(Func<TKey, TValue, bool> condition,
            IDictionaryConditionManager<TKey, TValue> manager)
        {
            var c = new DictionaryCondition<TKey, TValue>(condition);
            manager.Add(c);
            return c;
        }

        private static IDictionaryCondition<TKey, TValue> CreateCondition(Func<KeyValuePair<TKey, TValue>, bool> condition,
            IDictionaryConditionManager<TKey, TValue> manager)
        {
            var c = new DictionaryCondition<TKey, TValue>(condition);
            manager.Add(c);
            return c;
        }
    }
}
