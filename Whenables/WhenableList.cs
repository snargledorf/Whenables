using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Whenables.Core;

namespace Whenables
{
    public class WhenableList<T> : IWhenableList<T>
    {
        private readonly List<T> list;

        private readonly WheneableItemIndexPairConditionManager<T> addManager = new();
        private readonly WheneableItemIndexPairConditionManager<T> removeManager = new();
        private readonly WheneableItemIndexPairConditionManager<T> insertManager = new();

        private static readonly object lockObj = new();

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
            get => list[index];
            set => Insert(index, value);
        }

        public int Count => list.Count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            list.Add(item);
            TrySet(item, list.Count - 1, addManager);
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
                TrySet(item, index, removeManager);
            }
            return false;
        }

        public int IndexOf(T item) => list.IndexOf(item);

        public void Insert(int index, T item)
        {
            list.Insert(index, item);
            TrySet(item, index, insertManager);
        }

        public void RemoveAt(int index)
        {
            T item = list[index];
            list.RemoveAt(index);
            TrySet(item, index, removeManager);
        }

        public IEnumerator<T> GetEnumerator() => list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)list).GetEnumerator();

        public Task<T> WhenAddedAsync(Func<T, bool> condition)
            => WhenAddedAsync((t, _) => condition(t));

        public Task<T> WhenAddedAsync(Func<T, bool> condition, CancellationToken cancellationToken)
            => WhenAddedAsync((t, _) => condition(t), cancellationToken);

        public Task<T> WhenAddedAsync(Func<T, int, bool> condition)
            => WhenAddedAsync(condition, CancellationToken.None);

        public Task<T> WhenAddedAsync(Func<T, int, bool> condition, CancellationToken cancellationToken)
            => CreateConditionAsync(condition, addManager, cancellationToken);

        public Task<T> WhenInsertedAsync(Func<T, bool> condition)
            => WhenInsertedAsync((t, _) => condition(t));

        public Task<T> WhenInsertedAsync(Func<T, bool> condition, CancellationToken cancellationToken)
            => WhenInsertedAsync((t, _) => condition(t), CancellationToken.None);

        public Task<T> WhenInsertedAsync(Func<T, int, bool> condition)
            => WhenInsertedAsync(condition, CancellationToken.None);

        public Task<T> WhenInsertedAsync(Func<T, int, bool> condition, CancellationToken cancellationToken)
            => CreateConditionAsync(condition, insertManager, cancellationToken);

        public Task<T> WhenRemovedAsync(Func<T, bool> condition)
            => WhenRemovedAsync((t, _) => condition(t));

        public Task<T> WhenRemovedAsync(Func<T, bool> condition, CancellationToken cancellationToken)
            => WhenRemovedAsync((t, _) => condition(t), cancellationToken);

        public Task<T> WhenRemovedAsync(Func<T, int, bool> condition)
            => WhenRemovedAsync(condition, CancellationToken.None);

        public Task<T> WhenRemovedAsync(Func<T, int, bool> condition, CancellationToken cancellationToken)
            => CreateConditionAsync(condition, removeManager, cancellationToken);

        private static async Task<T> CreateConditionAsync(Func<T, int, bool> condition, IWheneableItemIndexPairConditionManager<T> manager, CancellationToken cancellationToken)
        {
            TaskCompletionSource<ItemIndexPair<T>> tcs;
            lock (lockObj)
            {
                tcs = manager.AddCondition(condition);
                cancellationToken.Register(() => tcs.TrySetCanceled());
            }

            ItemIndexPair<T> result = await tcs.Task;
            return result.Item;
        }

        private static void TrySet(T value, int index, IWheneableItemIndexPairConditionManager<T> manager)
        {
            lock (lockObj)
                manager.TrySet(value, index);
        }
    }
}
