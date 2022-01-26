using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Whenables.Core
{
    public interface IWhenableDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        Task<KeyValuePair<TKey, TValue>> WhenAddedAsync(Func<TKey, bool> condition);
        Task<KeyValuePair<TKey, TValue>> WhenAddedAsync(Func<TKey, bool> condition, CancellationToken cancellationToken);
        Task<KeyValuePair<TKey, TValue>> WhenAddedAsync(Func<TValue, bool> condition);
        Task<KeyValuePair<TKey, TValue>> WhenAddedAsync(Func<TValue, bool> condition, CancellationToken cancellationToken);
        Task<KeyValuePair<TKey, TValue>> WhenAddedAsync(Func<TKey, TValue, bool> condition);
        Task<KeyValuePair<TKey, TValue>> WhenAddedAsync(Func<TKey, TValue, bool> condition, CancellationToken cancellationToken);
        Task<KeyValuePair<TKey, TValue>> WhenAddedAsync(Func<KeyValuePair<TKey, TValue>, bool> condition);
        Task<KeyValuePair<TKey, TValue>> WhenAddedAsync(Func<KeyValuePair<TKey, TValue>, bool> condition, CancellationToken cancellationToken);
        Task<KeyValuePair<TKey, TValue>> WhenInsertedAsync(Func<TKey, bool> condition);
        Task<KeyValuePair<TKey, TValue>> WhenInsertedAsync(Func<TKey, bool> condition, CancellationToken cancellationToken);
        Task<KeyValuePair<TKey, TValue>> WhenInsertedAsync(Func<TValue, bool> condition);
        Task<KeyValuePair<TKey, TValue>> WhenInsertedAsync(Func<TValue, bool> condition, CancellationToken cancellationToken);
        Task<KeyValuePair<TKey, TValue>> WhenInsertedAsync(Func<TKey, TValue, bool> condition);
        Task<KeyValuePair<TKey, TValue>> WhenInsertedAsync(Func<TKey, TValue, bool> condition, CancellationToken cancellationToken);
        Task<KeyValuePair<TKey, TValue>> WhenInsertedAsync(Func<KeyValuePair<TKey, TValue>, bool> condition);
        Task<KeyValuePair<TKey, TValue>> WhenInsertedAsync(Func<KeyValuePair<TKey, TValue>, bool> condition, CancellationToken cancellationToken);
        Task<KeyValuePair<TKey, TValue>> WhenRemovedAsync(Func<TKey, bool> condition);
        Task<KeyValuePair<TKey, TValue>> WhenRemovedAsync(Func<TKey, bool> condition, CancellationToken cancellationToken);
        Task<KeyValuePair<TKey, TValue>> WhenRemovedAsync(Func<TValue, bool> condition);
        Task<KeyValuePair<TKey, TValue>> WhenRemovedAsync(Func<TValue, bool> condition, CancellationToken cancellationToken);
        Task<KeyValuePair<TKey, TValue>> WhenRemovedAsync(Func<TKey, TValue, bool> condition);
        Task<KeyValuePair<TKey, TValue>> WhenRemovedAsync(Func<TKey, TValue, bool> condition, CancellationToken cancellationToken);
        Task<KeyValuePair<TKey, TValue>> WhenRemovedAsync(Func<KeyValuePair<TKey, TValue>, bool> condition);
        Task<KeyValuePair<TKey, TValue>> WhenRemovedAsync(Func<KeyValuePair<TKey, TValue>, bool> condition, CancellationToken cancellationToken);
    }
}