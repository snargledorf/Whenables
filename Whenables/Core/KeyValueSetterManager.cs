using System.Collections.Generic;

namespace Whenables.Core
{
    internal class KeyValueSetterManager<TKey, TValue> : ResultSetterManager<KeyValuePair<TKey, TValue>>, IKeyValueSetterManager<TKey, TValue>
    {
        public void TrySet(TKey key, TValue value)
        {
            TrySetResult(new KeyValuePair<TKey, TValue>(key, value));
        }
    }
}