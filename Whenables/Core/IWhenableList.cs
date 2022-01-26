using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Whenables.Core
{
    public interface IWhenableList<T> : IList<T>
    {
        Task<T> WhenAddedAsync(Func<T, bool> condition);
        Task<T> WhenAddedAsync(Func<T, bool> condition, CancellationToken cancellationToken);
        Task<T> WhenAddedAsync(Func<T, int, bool> condition);
        Task<T> WhenAddedAsync(Func<T, int, bool> condition, CancellationToken cancellationToken);
        Task<T> WhenInsertedAsync(Func<T, bool> condition);
        Task<T> WhenInsertedAsync(Func<T, bool> condition, CancellationToken cancellationToken);
        Task<T> WhenInsertedAsync(Func<T, int, bool> condition);
        Task<T> WhenInsertedAsync(Func<T, int, bool> condition, CancellationToken cancellationToken);
        Task<T> WhenRemovedAsync(Func<T, bool> condition);
        Task<T> WhenRemovedAsync(Func<T, bool> condition, CancellationToken cancellationToken);
        Task<T> WhenRemovedAsync(Func<T, int, bool> condition);
        Task<T> WhenRemovedAsync(Func<T, int, bool> condition, CancellationToken cancellationToken);
    }
}