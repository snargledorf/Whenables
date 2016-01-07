using System;
using System.Collections.Generic;

namespace Whenables.Core
{
    public interface IWhenableList<T> : IList<T>
    {
        IResultAccessor<T> WhenAdded(Func<T, bool> condition);
        IResultAccessor<T> WhenAdded(Func<T, int, bool> condition);
        IResultAccessor<T> WhenInserted(Func<T, bool> condition);
        IResultAccessor<T> WhenInserted(Func<T, int, bool> condition);
        IResultAccessor<T> WhenRemoved(Func<T, bool> condition);
        IResultAccessor<T> WhenRemoved(Func<T, int, bool> condition);
    }
}