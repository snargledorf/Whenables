using System;
using System.Collections.Generic;

namespace Whenables.Core
{
    public interface IWhenableDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        IKeyValueResultAccessor<TKey, TValue> WhenAdded(Func<TKey, bool> condition);
        IKeyValueResultAccessor<TKey, TValue> WhenAdded(Func<TValue, bool> condition);
        IKeyValueResultAccessor<TKey, TValue> WhenAdded(Func<TKey, TValue, bool> condition);
        IKeyValueResultAccessor<TKey, TValue> WhenAdded(Func<KeyValuePair<TKey, TValue>, bool> condition);
        IKeyValueResultAccessor<TKey, TValue> WhenInserted(Func<TKey, bool> condition);
        IKeyValueResultAccessor<TKey, TValue> WhenInserted(Func<TValue, bool> condition);
        IKeyValueResultAccessor<TKey, TValue> WhenInserted(Func<TKey, TValue, bool> condition);
        IKeyValueResultAccessor<TKey, TValue> WhenInserted(Func<KeyValuePair<TKey, TValue>, bool> condition);
        IKeyValueResultAccessor<TKey, TValue> WhenRemoved(Func<TKey, bool> condition);
        IKeyValueResultAccessor<TKey, TValue> WhenRemoved(Func<TValue, bool> condition);
        IKeyValueResultAccessor<TKey, TValue> WhenRemoved(Func<TKey, TValue, bool> condition);
        IKeyValueResultAccessor<TKey, TValue> WhenRemoved(Func<KeyValuePair<TKey, TValue>, bool> condition);
    }
}