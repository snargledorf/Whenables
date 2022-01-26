using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Whenables.Core
{
    internal class WhenableConditionManager<T> : IWhenableConditionManager<T>
    {
        private readonly Dictionary<Func<T, bool>, TaskCompletionSource<T>> conditionsToTcs = new();

        public TaskCompletionSource<T> AddCondition(Func<T, bool> condition)
        {
            return conditionsToTcs[condition] = new TaskCompletionSource<T>();
        }

        public void TrySet(T item)
        {
            foreach (var conditionAndTcs in conditionsToTcs)
            {
                if (conditionAndTcs.Key(item))
                {
                    conditionsToTcs.Remove(conditionAndTcs.Key);
                    conditionAndTcs.Value.TrySetResult(item);
                }
            }
        }
    }
}