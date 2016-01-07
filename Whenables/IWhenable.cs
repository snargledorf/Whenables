using System;

namespace Whenables
{
    public interface IWhenable<T>
    {
        ICondition<T> When(Func<T, bool> condition);
    }
}