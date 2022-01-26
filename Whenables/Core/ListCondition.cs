using System;
using System.Threading;
using System.Threading.Tasks;

namespace Whenables.Core
{
    public class ListCondition<T> : IListResultSetter<T>, IResultAccessor<T>
    {
        private readonly Func<T, int, bool> condition;

        private TaskCompletionSource<T> tcs = new();

        public ListCondition(Func<T, bool> condition)
            : this((t, i) => condition(t))
        {
        }

        public ListCondition(Func<T, int, bool> condition)
        {
            this.condition = condition;
        }

        public T Result => tcs.Task.Result;

        public bool HasResult => tcs.Task.IsCompletedSuccessfully;

        public bool TrySetResult(T result, int index)
        {
            if (!condition(result, index))
                return false;

            SetResult(result);

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
            cancellationToken.Register(() => tcs.TrySetCanceled());
            return tcs.Task;
        }

        protected void SetResult(T result)
        {
            tcs.TrySetResult(result);
        }
    }
}