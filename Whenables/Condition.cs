using System;
using System.Threading;
using System.Threading.Tasks;

namespace Whenables
{
    public class Condition<T> : ICondition<T>
    {
        private readonly object sync = new object();

        private readonly Func<T, bool> condition;

        private TaskCompletionSource<T> tcs;

        public Condition(Func<T, bool> condition)
        {
            this.condition = condition;
        }

        public bool HasItem { get; private set; }

        public T Item { get; private set; }

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

        public bool TrySetItem(T item)
        {
            if (!condition(item))
                return false;

            lock (sync)
            {
                Item = item;
                HasItem = true;

                tcs?.TrySetResult(item);
            }

            return true;
        }
    }
}