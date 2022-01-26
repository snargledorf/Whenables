using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Whenables.Core;

namespace Whenables
{
    public class Whenable<T> : IWhenable<T>
    {
        private readonly WhenableConditionManager<T> manager = new();

        private static readonly object lockObj = new();

        private T value;

        public Whenable()
        {
        }

        public Whenable(T value)
        {
            this.value = value;
        }

        public T Value
        {
            get { return value; }
            set
            {
                this.value = value;
                TrySet(value);
            }
        }

        public Task<T> WhenAsync(Func<T, bool> condition) => WhenAsync(condition, CancellationToken.None);

        public Task<T> WhenAsync(Func<T, bool> condition, CancellationToken cancellationToken) => CreateValueTaskForCondition(condition, cancellationToken);

        private Task<T> CreateValueTaskForCondition(Func<T, bool> condition, CancellationToken cancellationToken)
        {
            lock (lockObj)
            {
                TaskCompletionSource<T> tcs = manager.AddCondition(condition);
                cancellationToken.Register(() => tcs.TrySetCanceled(cancellationToken));

                return tcs.Task;
            }
        }

        private void TrySet(T item)
        {
            lock (lockObj)
                manager.TrySet(item);
        }

        public static implicit operator T(Whenable<T> source)
        {
            return source.Value;
        }
    }
}
