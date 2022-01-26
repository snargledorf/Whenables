using System.Collections.Generic;

namespace Whenables.Core
{
    internal class WhenableKeyValuePairConditionManager<TKey, TValue> : WhenableConditionManager<KeyValuePair<TKey, TValue>>, IWhenableKeyValuePairConditionManager<TKey, TValue>
    {
        public void TrySet(TKey key, TValue value) => TrySet(new KeyValuePair<TKey, TValue>(key, value));
    }
}