using System;
using System.Threading;
using System.Threading.Tasks;

namespace Whenables
{
    public class ListCondition<T> : IListCondition<T>
    {
        private readonly object sync = new object();

        private readonly Func<T, int, bool> condition;

        private TaskCompletionSource<T> tcs;

        public ListCondition(Func<T, bool> condition)
            : this((t, i) => condition(t))
        {
        }

        public ListCondition(Func<T, int, bool> condition)
        {
            this.condition = condition;
        }

        public T Item { get; private set; }

        public bool HasItem { get; private set; }

        public bool TrySetItem(T item, int index)
        {
            if (!condition(item, index))
                return false;

            lock (sync)
            {
                Item = item;
                HasItem = true;

                tcs?.TrySetResult(item);
            }

            return true;
        }

        public T Get() => Get(CancellationToken.None);
        public T Get(TimeSpan timeout) => Get(new CancellationTokenSource(timeout).Token);
        public T Get(int timoutMilliseconds) => Get(TimeSpan.FromMilliseconds(timoutMilliseconds));
        public T Get(CancellationToken cancellationToken) => GetAsync(cancellationToken).Result;

        public Task<T> GetAsync() => GetAsync(CancellationToken.None);
        public Task<T> GetAsync(TimeSpan timeout) => GetAsync(new CancellationTokenSource(timeout).Token);
        public Task<T> GetAsync(int timeoutMilliseconds) => GetAsync(TimeSpan.FromMilliseconds(timeoutMilliseconds));
        public Task<T> GetAsync(CancellationToken cancellationToken)
        {
            lock (sync)
            {
                if (HasItem)
                    return Task.FromResult(Item);

                tcs = new TaskCompletionSource<T>();

                cancellationToken.Register(() => tcs?.TrySetCanceled());

                return tcs.Task;
            }
        }
    }
}