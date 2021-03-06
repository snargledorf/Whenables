using System;
using System.Threading;
using System.Threading.Tasks;

namespace Whenables.Core
{
    public interface IResultAccessor<T>
    {
        T Result { get; }
        bool HasResult { get; }

        T Get();
        T Get(TimeSpan timeout);
        T Get(int timoutMilliseconds);
        T Get(CancellationToken cancellationToken);

        Task<T> GetAsync();
        Task<T> GetAsync(TimeSpan timeout);
        Task<T> GetAsync(int timeoutMilliseconds);
        Task<T> GetAsync(CancellationToken cancellationToken);
    }
}