using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Whenables.Core
{
    public interface IKeyValueResultAccessor<TKey, TValue> : IResultAccessor<KeyValuePair<TKey, TValue>>
    {
        TValue GetValue();
        TValue GetValue(TimeSpan timeout);
        TValue GetValue(int timeoutMilliseconds);
        TValue GetValue(CancellationToken cancellationToken);

        Task<TValue> GetValueAsync();
        Task<TValue> GetValueAsync(TimeSpan timeout);
        Task<TValue> GetValueAsync(int timeoutMilliseconds);
        Task<TValue> GetValueAsync(CancellationToken cancellationToken);

        TKey GetKey();
        TKey GetKey(TimeSpan timeout);
        TKey GetKey(int timeoutMilliseconds);
        TKey GetKey(CancellationToken cancellationToken);

        Task<TKey> GetKeyAsync();
        Task<TKey> GetKeyAsync(TimeSpan timeout);
        Task<TKey> GetKeyAsync(int timeoutMilliseconds);
        Task<TKey> GetKeyAsync(CancellationToken cancellationToken);
    }
}