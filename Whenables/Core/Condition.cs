using System;
using System.Threading;
using System.Threading.Tasks;

namespace Whenables.Core
{
    public class Condition<T> : IResultAccessor<T>, IResultSetter<T>
    {
        private readonly object sync = new object();

        private readonly Func<T, bool> condition;

        private TaskCompletionSource<T> tcs;

        public Condition(Func<T, bool> condition)
        {
            this.condition = condition;
        }

        public bool HasResult { get; private set; }

        public T Result { get; private set; }

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
                if (HasResult)
                    return Task.FromResult(Result);

                tcs = new TaskCompletionSource<T>();

                cancellationToken.Register(() => tcs?.TrySetCanceled());

                return tcs.Task;
            }
        }

        public bool TrySetResult(T result)
        {
            if (!condition(result))
                return false;

            SetResult(result);
            return true;
        }

        protected void SetResult(T result)
        {
            lock (sync)
            {
                Result = result;
                HasResult = true;

                tcs?.TrySetResult(result);
            }
        }
    }
}