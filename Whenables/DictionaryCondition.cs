using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Whenables
{
    public class DictionaryCondition<TKey, TValue> : IDictionaryCondition<TKey, TValue>
    {
        private readonly object sync = new object();

        private readonly Func<TKey, TValue, bool> condition;

        private TaskCompletionSource<KeyValuePair<TKey, TValue>> tcs;

        public DictionaryCondition(Func<TKey, bool> condition)
            : this((key, value) => condition(key))
        {
        }

        public DictionaryCondition(Func<TValue, bool> condition)
            : this((key, value) => condition(value))
        {
        }

        public DictionaryCondition(Func<KeyValuePair<TKey, TValue>, bool> condition)
            : this((key, value) => condition(new KeyValuePair<TKey, TValue>(key, value)))
        {
        }

        public DictionaryCondition(Func<TKey, TValue, bool> condition)
        {
            this.condition = condition;
        }

        public KeyValuePair<TKey, TValue>? KeyAndValue { get; private set; }

        public TValue GetValue() => GetValue(CancellationToken.None);
        public TValue GetValue(TimeSpan timeout) => GetValue(new CancellationTokenSource(timeout).Token);
        public TValue GetValue(int timeoutMilliseconds) => GetValue(TimeSpan.FromMilliseconds(timeoutMilliseconds));
        public TValue GetValue(CancellationToken cancellationToken) => GetValueAsync(cancellationToken).Result;

        public Task<TValue> GetValueAsync() => GetValueAsync(CancellationToken.None);
        public Task<TValue> GetValueAsync(TimeSpan timeout) => GetValueAsync(new CancellationTokenSource(timeout).Token);
        public Task<TValue> GetValueAsync(int timeoutMilliseconds) => GetValueAsync(TimeSpan.FromMilliseconds(timeoutMilliseconds));
        public async Task<TValue> GetValueAsync(CancellationToken cancellationToken)
        {
            KeyValuePair<TKey, TValue> valuePair = await GetKeyValuePairAsync(cancellationToken);
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
            KeyValuePair<TKey, TValue> valuePair = await GetKeyValuePairAsync(cancellationToken);
            return valuePair.Key;
        }

        public KeyValuePair<TKey, TValue> GetKeyValuePair() => GetKeyValuePair(CancellationToken.None);
        public KeyValuePair<TKey, TValue> GetKeyValuePair(TimeSpan timeout) => GetKeyValuePair(new CancellationTokenSource(timeout).Token);
        public KeyValuePair<TKey, TValue> GetKeyValuePair(int timeoutMilliseconds) => GetKeyValuePair(TimeSpan.FromMilliseconds(timeoutMilliseconds));
        public KeyValuePair<TKey, TValue> GetKeyValuePair(CancellationToken cancellationToken) => GetKeyValuePairAsync(cancellationToken).Result;

        public Task<KeyValuePair<TKey, TValue>> GetKeyValuePairAsync() => GetKeyValuePairAsync(CancellationToken.None);
        public Task<KeyValuePair<TKey, TValue>> GetKeyValuePairAsync(TimeSpan timeout) => GetKeyValuePairAsync(new CancellationTokenSource(timeout).Token);
        public Task<KeyValuePair<TKey, TValue>> GetKeyValuePairAsync(int timeoutMilliseconds) => GetKeyValuePairAsync(TimeSpan.FromMilliseconds(timeoutMilliseconds));
        public Task<KeyValuePair<TKey, TValue>> GetKeyValuePairAsync(CancellationToken cancellationToken)
        {
            lock (sync)
            {
                if (KeyAndValue != null)
                    return Task.FromResult(KeyAndValue.Value);

                tcs = new TaskCompletionSource<KeyValuePair<TKey, TValue>>();

                cancellationToken.Register(() => tcs?.TrySetCanceled());

                return tcs.Task;
            }
        }

        public bool TrySetKeyAndValue(TKey key, TValue value)
        {
            if (!condition(key, value))
                return false;

            lock (sync)
            {
                KeyAndValue = new KeyValuePair<TKey, TValue>(key, value);
                tcs?.TrySetResult(KeyAndValue.Value);

                return true;
            }
        }
    }
}