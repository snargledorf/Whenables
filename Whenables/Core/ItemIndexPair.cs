using System;
using System.Collections.Generic;

namespace Whenables.Core
{
    internal struct ItemIndexPair<T>
    {
        public T Item { get; }

        public int Index { get; }

        public ItemIndexPair(T item, int index) : this()
        {
            Item = item;
            Index = index;
        }

        public override bool Equals(object obj)
        {
            return obj is ItemIndexPair<T> pair &&
                   EqualityComparer<T>.Default.Equals(Item, pair.Item) &&
                   Index == pair.Index;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Item, Index);
        }
    }
}