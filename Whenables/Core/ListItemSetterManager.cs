using System.Collections.Generic;
using System.Linq;

namespace Whenables.Core
{
    public class ListItemSetterManager<T> : IListItemSetterManager<T>
    {
        private readonly HashSet<IListResultSetter<T>> conditions = new HashSet<IListResultSetter<T>>();

        public void Add(IListResultSetter<T> listResultSetter)
        {
            conditions.Add(listResultSetter);
        }

        public void Remove(IListResultSetter<T> listResultSetter)
        {
            conditions.Remove(listResultSetter);
        }

        public void TrySet(T item, int index)
        {
            foreach (IListResultSetter<T> condition in conditions.ToArray())
            {
                if (condition.TrySetResult(item, index))
                    conditions.Remove(condition);
            }
        }
    }
}