using System;

namespace Whenables.Core
{
    public interface IWhenable<T>
    {
        IResultAccessor<T> When(Func<T, bool> condition);
    }
}