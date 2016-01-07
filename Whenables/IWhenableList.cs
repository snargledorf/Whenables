using System;
using System.Collections.Generic;

namespace Whenables
{
    public interface IWhenableList<T> : IList<T>
    {
        IListCondition<T> WhenAdded(Func<T, bool> condition);
        IListCondition<T> WhenAdded(Func<T, int, bool> condition);
        IListCondition<T> WhenInserted(Func<T, bool> condition);
        IListCondition<T> WhenInserted(Func<T, int, bool> condition);
        IListCondition<T> WhenRemoved(Func<T, bool> condition);
        IListCondition<T> WhenRemoved(Func<T, int, bool> condition);
    }
}