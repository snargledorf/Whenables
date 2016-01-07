using System.Collections.Generic;

namespace Whenables.Core
{
    internal interface IKeyValueSetterManager<TKey, TValue> : IResultSetterManager<KeyValuePair<TKey, TValue>>
    {
        void TrySet(TKey key, TValue value);
    }
}