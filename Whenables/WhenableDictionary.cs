using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Whenables.Core;

namespace Whenables
{
    public class WhenableDictionary<TKey, TValue> : IWhenableDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> dict;

        private readonly WhenableKeyValuePairConditionManager<TKey, TValue> addManager = new();
        private readonly WhenableKeyValuePairConditionManager<TKey, TValue> removeManager = new();
        private readonly WhenableKeyValuePairConditionManager<TKey, TValue> insertManager = new();

        private static readonly object lockObj = new();

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
                TrySet(key, value, insertManager);
            }
        }

        public ICollection<TKey> Keys => dict.Keys;

        public ICollection<TValue> Values => dict.Values;

        public int Count => dict.Count;

        public bool IsReadOnly => dict.IsReadOnly;

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            dict.Add(item);
            TrySet(item, addManager);
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

            TrySet(item, removeManager);

            return true;
        }

        public bool ContainsKey(TKey key) => dict.ContainsKey(key);

        public void Add(TKey key, TValue value)
        {
            dict.Add(key, value);
            TrySet(key, value, addManager);
        }

        public bool Remove(TKey key)
        {
            if (!ContainsKey(key))
                return false;

            TValue value = this[key];

            if (!dict.Remove(key))
                return false;

            TrySet(key, value, removeManager);

            return true;
        }

        public bool TryGetValue(TKey key, out TValue value) => dict.TryGetValue(key, out value);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => dict.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)dict).GetEnumerator();

        public Task<KeyValuePair<TKey, TValue>> WhenAddedAsync(Func<TKey, bool> condition)
            => WhenAddedAsync(kvp => condition(kvp.Key));

        public Task<KeyValuePair<TKey, TValue>> WhenAddedAsync(Func<TKey, bool> condition, CancellationToken cancellationToken)
            => WhenAddedAsync(kvp => condition(kvp.Key), cancellationToken);

        public Task<KeyValuePair<TKey, TValue>> WhenAddedAsync(Func<TValue, bool> condition)
            => WhenAddedAsync(kvp => condition(kvp.Value));

        public Task<KeyValuePair<TKey, TValue>> WhenAddedAsync(Func<TValue, bool> condition, CancellationToken cancellationToken)
            => WhenAddedAsync(kvp => condition(kvp.Value), cancellationToken);

        public Task<KeyValuePair<TKey, TValue>> WhenAddedAsync(Func<TKey, TValue, bool> condition)
            => WhenAddedAsync(kvp => condition(kvp.Key, kvp.Value));

        public Task<KeyValuePair<TKey, TValue>> WhenAddedAsync(Func<TKey, TValue, bool> condition, CancellationToken cancellationToken)
            => WhenAddedAsync(kvp => condition(kvp.Key, kvp.Value), cancellationToken);

        public Task<KeyValuePair<TKey, TValue>> WhenAddedAsync(Func<KeyValuePair<TKey, TValue>, bool> condition)
            => WhenAddedAsync(condition, CancellationToken.None);

        public Task<KeyValuePair<TKey, TValue>> WhenAddedAsync(Func<KeyValuePair<TKey, TValue>, bool> condition, CancellationToken cancellationToken)
            => CreateConditionAsync(condition, addManager, cancellationToken);

        public Task<KeyValuePair<TKey, TValue>> WhenInsertedAsync(Func<TKey, bool> condition)
            => WhenInsertedAsync(kvp => condition(kvp.Key));

        public Task<KeyValuePair<TKey, TValue>> WhenInsertedAsync(Func<TKey, bool> condition, CancellationToken cancellationToken)
            => WhenInsertedAsync(kvp => condition(kvp.Key), cancellationToken);

        public Task<KeyValuePair<TKey, TValue>> WhenInsertedAsync(Func<TValue, bool> condition)
            => WhenInsertedAsync(kvp => condition(kvp.Value));

        public Task<KeyValuePair<TKey, TValue>> WhenInsertedAsync(Func<TValue, bool> condition, CancellationToken cancellationToken)
            => WhenInsertedAsync(kvp => condition(kvp.Value), cancellationToken);

        public Task<KeyValuePair<TKey, TValue>> WhenInsertedAsync(Func<TKey, TValue, bool> condition)
            => WhenInsertedAsync(kvp => condition(kvp.Key, kvp.Value));

        public Task<KeyValuePair<TKey, TValue>> WhenInsertedAsync(Func<TKey, TValue, bool> condition, CancellationToken cancellationToken)
            => WhenInsertedAsync(kvp => condition(kvp.Key, kvp.Value), cancellationToken);

        public Task<KeyValuePair<TKey, TValue>> WhenInsertedAsync(Func<KeyValuePair<TKey, TValue>, bool> condition)
            => WhenInsertedAsync(condition, CancellationToken.None);

        public Task<KeyValuePair<TKey, TValue>> WhenInsertedAsync(Func<KeyValuePair<TKey, TValue>, bool> condition, CancellationToken cancellationToken)
            => CreateConditionAsync(condition, insertManager, cancellationToken);

        public Task<KeyValuePair<TKey, TValue>> WhenRemovedAsync(Func<TKey, bool> condition)
            => WhenRemovedAsync(kvp => condition(kvp.Key));

        public Task<KeyValuePair<TKey, TValue>> WhenRemovedAsync(Func<TValue, bool> condition)
            => WhenRemovedAsync(kvp => condition(kvp.Value));

        public Task<KeyValuePair<TKey, TValue>> WhenRemovedAsync(Func<TKey, TValue, bool> condition)
            => WhenRemovedAsync(kvp => condition(kvp.Key, kvp.Value));

        public Task<KeyValuePair<TKey, TValue>> WhenRemovedAsync(Func<KeyValuePair<TKey, TValue>, bool> condition)
            => WhenRemovedAsync(condition, CancellationToken.None);

        public Task<KeyValuePair<TKey, TValue>> WhenRemovedAsync(Func<TKey, bool> condition, CancellationToken cancellationToken)
            => WhenRemovedAsync(kvp => condition(kvp.Key), cancellationToken);

        public Task<KeyValuePair<TKey, TValue>> WhenRemovedAsync(Func<TValue, bool> condition, CancellationToken cancellationToken)
            => WhenRemovedAsync(kvp => condition(kvp.Value), cancellationToken);

        public Task<KeyValuePair<TKey, TValue>> WhenRemovedAsync(Func<TKey, TValue, bool> condition, CancellationToken cancellationToken)
            => WhenRemovedAsync(kvp => condition(kvp.Key, kvp.Value), cancellationToken);

        public Task<KeyValuePair<TKey, TValue>> WhenRemovedAsync(Func<KeyValuePair<TKey, TValue>, bool> condition, CancellationToken cancellationToken)
            => CreateConditionAsync(condition, removeManager, cancellationToken);

        
        private static Task<KeyValuePair<TKey, TValue>> CreateConditionAsync(Func<KeyValuePair<TKey, TValue>, bool> condition,
            IWhenableConditionManager<KeyValuePair<TKey, TValue>> manager, CancellationToken cancellationToken)
        {
            lock (lockObj)
            {
                TaskCompletionSource<KeyValuePair<TKey, TValue>> tcs = manager.AddCondition(condition);
                cancellationToken.Register(() => tcs.TrySetCanceled());
                return tcs.Task;
            }
        }

        private static void TrySet(TKey key, TValue value, IWhenableKeyValuePairConditionManager<TKey, TValue> manager)
        {
            lock (lockObj)
                manager.TrySet(key, value);
        }

        private static void TrySet(KeyValuePair<TKey, TValue> item, IWhenableKeyValuePairConditionManager<TKey, TValue> manager)
        {
            lock (lockObj)
                manager.TrySet(item);
        }
    }
}
