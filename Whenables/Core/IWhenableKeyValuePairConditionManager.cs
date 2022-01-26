using System.Collections.Generic;

namespace Whenables.Core
{
    internal interface IWhenableKeyValuePairConditionManager<TKey, TValue> : IWhenableConditionManager<KeyValuePair<TKey, TValue>>
    {
        void TrySet(TKey key, TValue value);
    }
}