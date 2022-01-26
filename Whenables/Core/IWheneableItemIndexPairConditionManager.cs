using System;
using System.Threading.Tasks;

namespace Whenables.Core
{
    internal interface IWheneableItemIndexPairConditionManager<T> : IWhenableConditionManager<ItemIndexPair<T>>
    {
        TaskCompletionSource<ItemIndexPair<T>> AddCondition(Func<T, int, bool> condition);
        void TrySet(T item, int index);
    }
}