using System;
using System.Collections;
using System.Collections.Generic;
using Whenables.Core;

namespace Whenables
{
    public class WhenableList<T> : IWhenableList<T>
    {
        private readonly IList<T> list;

        private readonly ListItemSetterManager<T> addManager = new ListItemSetterManager<T>();
        private readonly ListItemSetterManager<T> removeManager = new ListItemSetterManager<T>();
        private readonly ListItemSetterManager<T> insertManager = new ListItemSetterManager<T>();

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
                insertManager.TrySet(value, index);
            }
        }

        public int Count => list.Count;

        public bool IsReadOnly => list.IsReadOnly;

        public void Add(T item)
        {
            list.Add(item);
            addManager.TrySet(item, list.Count-1);
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
                removeManager.TrySet(item, index);
            }
            return false;
        }

        public int IndexOf(T item) => list.IndexOf(item);

        public void Insert(int index, T item)
        {
            list.Insert(index, item);
            insertManager.TrySet(item, index);
        }

        public void RemoveAt(int index)
        {
            T item = list[index];
            list.RemoveAt(index);
            removeManager.TrySet(item, index);
        }

        public IEnumerator<T> GetEnumerator() => list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)list).GetEnumerator();

        public IResultAccessor<T> WhenAdded(Func<T, bool> condition) 
            => WhenAdded((t, i) => condition(t));
        public IResultAccessor<T> WhenAdded(Func<T, int, bool> condition) 
            => CreateAddInsertCondition(condition, addManager);

        public IResultAccessor<T> WhenInserted(Func<T, bool> condition)
            => WhenInserted((t, i) => condition(t));
        public IResultAccessor<T> WhenInserted(Func<T, int, bool> condition)
            => CreateAddInsertCondition(condition, insertManager);

        public IResultAccessor<T> WhenRemoved(Func<T, bool> condition)
            => WhenRemoved((t, i) => condition(t));
        public IResultAccessor<T> WhenRemoved(Func<T, int, bool> condition)
            => CreateCondition(condition, removeManager);

        private IResultAccessor<T> CreateAddInsertCondition(Func<T, int, bool> condition, IListItemSetterManager<T> manager)
        {
            var c = new ListCondition<T>(condition);

            // Go through all of the existing items to see if this condition
            // has already been met. Otherwise the caller could potentially
            // wait forever.
            bool conditionNotMet = true;
            for (int index = 0; index < list.Count; index++)
                if (c.TrySetResult(list[index], index))
                    conditionNotMet = false;

            // The condition was not met by any of the items in the list. 
            // Add the condition to the manager to be monitored.
            if (conditionNotMet)
                manager.Add(c);

            return c;
        }

        private static IResultAccessor<T> CreateCondition(Func<T, int, bool> condition, IListItemSetterManager<T> manager)
        {
            var c = new ListCondition<T>(condition);
            manager.Add(c);
            return c;
        }
    }
}
