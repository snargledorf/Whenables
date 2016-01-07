using System.Collections.Generic;
using System.Linq;

namespace Whenables.Core
{
    internal class ResultSetterManager<T> : IResultSetterManager<T>
    {
        private readonly HashSet<IResultSetter<T>> setters = new HashSet<IResultSetter<T>>();

        public void Add(IResultSetter<T> setter)
        {
            setters.Add(setter);
        }

        public void Remove(IResultSetter<T> setter)
        {
            setters.Remove(setter);
        }

        public void TrySetResult(T item)
        {
            foreach (IResultSetter<T> setter in setters.ToArray())
            {
                if (setter.TrySetResult(item))
                    setters.Remove(setter);
            }
        }
    }
}