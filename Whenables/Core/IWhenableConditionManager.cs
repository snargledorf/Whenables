using System;
using System.Threading.Tasks;

namespace Whenables.Core
{
    public interface IWhenableConditionManager<T>
    {
        TaskCompletionSource<T> AddCondition(Func<T, bool> condition);
        void TrySet(T item);
    }
}