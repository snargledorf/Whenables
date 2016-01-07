using System;
using System.Collections;
using System.Collections.Generic;

namespace Whenables
{
    public class WhenableList<T> : IWhenableList<T>
    {
        private readonly IList<T> list;

        private readonly ListConditionManager<T> addingListConditions = new ListConditionManager<T>();
        private readonly ListConditionManager<T> removingListConditions = new ListConditionManager<T>();
        private readonly ListConditionManager<T> insertListConditions = new ListConditionManager<T>();

        public WhenableList()
        {
            list = new List<T>();
        }

        public WhenableList(IEnumerable<T> items)
        {
            list = new List<T>(items);
        }

        public WhenableList(int capacity)
        {
            list = new List<T>(capacity);
        }

        public T this[int index]
        {
            get { return list[index]; }
            set
            {
                list[index] = value;
                insertListConditions.TrySetItemOnConditions(value, index);
            }
        }

        public int Count => list.Count;

        public bool IsReadOnly => list.IsReadOnly;

        public void Add(T item)
        {
            list.Add(item);
            addingListConditions.TrySetItemOnConditions(item, list.Count-1);
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(T item) => list.Contains(item);

        public void CopyTo(T[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                list.RemoveAt(index);
                removingListConditions.TrySetItemOnConditions(item, index);
            }
            return false;
        }

        public int IndexOf(T item) => list.IndexOf(item);

        public void Insert(int index, T item)
        {
            list.Insert(index, item);
            insertListConditions.TrySetItemOnConditions(item, index);
        }

        public void RemoveAt(int index)
        {
            T item = list[index];
            list.RemoveAt(index);
            removingListConditions.TrySetItemOnConditions(item, index);
        }

        public IEnumerator<T> GetEnumerator() => list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)list).GetEnumerator();

        public IListCondition<T> WhenAdded(Func<T, bool> condition) => WhenAdded((t, i) => condition(t));
        public IListCondition<T> WhenAdded(Func<T, int, bool> condition) => CreateCondition(condition, addingListConditions);

        public IListCondition<T> WhenInserted(Func<T, bool> condition) => WhenInserted((t, i) => condition(t));
        public IListCondition<T> WhenInserted(Func<T, int, bool> condition) => CreateCondition(condition, insertListConditions);

        public IListCondition<T> WhenRemoved(Func<T, bool> condition) => WhenRemoved((t, i) => condition(t));
        public IListCondition<T> WhenRemoved(Func<T, int, bool> condition) => CreateCondition(condition, removingListConditions);

        private static IListCondition<T> CreateCondition(Func<T, int, bool> condition, IListConditionManager<T> manager)
        {
            var c = new ListCondition<T>(condition);
            manager.Add(c);
            return c;
        }
    }
}
