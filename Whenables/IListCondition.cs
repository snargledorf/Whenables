using System;
using System.Threading;
using System.Threading.Tasks;

namespace Whenables
{
    public interface IListCondition<T>
    {
        T Get();
        T Get(TimeSpan timeout);
        T Get(int timoutMilliseconds);
        T Get(CancellationToken cancellationToken);

        Task<T> GetAsync();
        Task<T> GetAsync(TimeSpan timeout);
        Task<T> GetAsync(int timeoutMilliseconds);
        Task<T> GetAsync(CancellationToken cancellationToken);

        bool TrySetItem(T item, int index);
    }
}