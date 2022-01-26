using System;
using System.Threading.Tasks;

namespace Whenables.Core
{
    internal class WheneableItemIndexPairConditionManager<T> : WhenableConditionManager<ItemIndexPair<T>>, IWheneableItemIndexPairConditionManager<T>
    {
        public TaskCompletionSource<ItemIndexPair<T>> AddCondition(Func<T, int, bool> condition)
            => AddCondition(iip => condition(iip.Item, iip.Index));

        public void TrySet(T item, int index) => TrySet(new ItemIndexPair<T>(item, index));
    }
}