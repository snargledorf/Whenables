using System.Collections.Generic;
using System.Linq;

namespace Whenables
{
    internal class ConditionManager<T> : IConditionManager<T>
    {
        private readonly HashSet<ICondition<T>> conditions = new HashSet<ICondition<T>>();

        public void Add(ICondition<T> condition)
        {
            conditions.Add(condition);
        }

        public void Remove(ICondition<T> condition)
        {
            conditions.Remove(condition);
        }

        public void TrySetItemOnConditions(T item)
        {
            foreach (ICondition<T> condition in conditions.ToArray())
            {
                if (condition.TrySetItem(item))
                    conditions.Remove(condition);
            }
        }
    }
}