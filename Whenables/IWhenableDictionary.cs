using System;
using System.Collections.Generic;

namespace Whenables
{
    public interface IWhenableDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        IDictionaryCondition<TKey, TValue> WhenAdded(Func<TKey, bool> condition);
        IDictionaryCondition<TKey, TValue> WhenAdded(Func<TValue, bool> condition);
        IDictionaryCondition<TKey, TValue> WhenAdded(Func<TKey, TValue, bool> condition);
        IDictionaryCondition<TKey, TValue> WhenAdded(Func<KeyValuePair<TKey, TValue>, bool> condition);
        IDictionaryCondition<TKey, TValue> WhenInserted(Func<TKey, bool> condition);
        IDictionaryCondition<TKey, TValue> WhenInserted(Func<TValue, bool> condition);
        IDictionaryCondition<TKey, TValue> WhenInserted(Func<TKey, TValue, bool> condition);
        IDictionaryCondition<TKey, TValue> WhenInserted(Func<KeyValuePair<TKey, TValue>, bool> condition);
        IDictionaryCondition<TKey, TValue> WhenRemoved(Func<TKey, bool> condition);
        IDictionaryCondition<TKey, TValue> WhenRemoved(Func<TValue, bool> condition);
        IDictionaryCondition<TKey, TValue> WhenRemoved(Func<TKey, TValue, bool> condition);
        IDictionaryCondition<TKey, TValue> WhenRemoved(Func<KeyValuePair<TKey, TValue>, bool> condition);
    }
}