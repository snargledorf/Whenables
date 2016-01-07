using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Whenables.Core
{
    public class KeyValueCondition<TKey, TValue> : Condition<KeyValuePair<TKey, TValue>>, IKeyValueResultAccessor<TKey, TValue>
    {
        public KeyValueCondition(Func<TKey, bool> condition)
            : this(kvp => condition(kvp.Key))
        {
        }

        public KeyValueCondition(Func<TValue, bool> condition)
            : this(kvp => condition(kvp.Value))
        {
        }

        public KeyValueCondition(Func<TKey, TValue, bool> condition)
            : this(kvp => condition(kvp.Key, kvp.Value))
        {
        }

        public KeyValueCondition(Func<KeyValuePair<TKey, TValue>, bool> condition)
            : base(condition)
        {
        }

        public TValue GetValue() => GetValue(CancellationToken.None);
        public TValue GetValue(TimeSpan timeout) => GetValue(new CancellationTokenSource(timeout).Token);
        public TValue GetValue(int timeoutMilliseconds) => GetValue(TimeSpan.FromMilliseconds(timeoutMilliseconds));
        public TValue GetValue(CancellationToken cancellationToken) => GetValueAsync(cancellationToken).Result;

        public Task<TValue> GetValueAsync() => GetValueAsync(CancellationToken.None);
        public Task<TValue> GetValueAsync(TimeSpan timeout) => GetValueAsync(new CancellationTokenSource(timeout).Token);
        public Task<TValue> GetValueAsync(int timeoutMilliseconds) => GetValueAsync(TimeSpan.FromMilliseconds(timeoutMilliseconds));
        public async Task<TValue> GetValueAsync(CancellationToken cancellationToken)
        {
            KeyValuePair<TKey, TValue> valuePair = await GetAsync(cancellationToken);
            return valuePair.Value;
        }

        public TKey GetKey() => GetKey(CancellationToken.None);
        public TKey GetKey(TimeSpan timeout) => GetKey(new CancellationTokenSource(timeout).Token);
        public TKey GetKey(int timeoutMilliseconds) => GetKey(TimeSpan.FromMilliseconds(timeoutMilliseconds));
        public TKey GetKey(CancellationToken cancellationToken) => GetKeyAsync(cancellationToken).Result;

        public Task<TKey> GetKeyAsync() => GetKeyAsync(CancellationToken.None);
        public Task<TKey> GetKeyAsync(TimeSpan timeout) => GetKeyAsync(new CancellationTokenSource(timeout).Token);
        public Task<TKey> GetKeyAsync(int timeoutMilliseconds) => GetKeyAsync(TimeSpan.FromMilliseconds(timeoutMilliseconds));
        public async Task<TKey> GetKeyAsync(CancellationToken cancellationToken)
        {
            KeyValuePair<TKey, TValue> valuePair = await GetAsync(cancellationToken);
            return valuePair.Key;
        }
    }
}