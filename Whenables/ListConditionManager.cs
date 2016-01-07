using System.Collections.Generic;
using System.Linq;

namespace Whenables
{
    public class ListConditionManager<T> : IListConditionManager<T>
    {
        private readonly HashSet<IListCondition<T>> conditions = new HashSet<IListCondition<T>>();

        public void Add(IListCondition<T> listCondition)
        {
            conditions.Add(listCondition);
        }

        public void Remove(IListCondition<T> listCondition)
        {
            conditions.Remove(listCondition);
        }

        public void TrySetItemOnConditions(T item, int index)
        {
            foreach (IListCondition<T> condition in conditions.ToArray())
            {
                if (condition.TrySetItem(item, index))
                    conditions.Remove(condition);
            }
        }
    }
}