namespace Whenables.Core
{
    public interface IResultSetterManager<T>
    {
        void Add(IResultSetter<T> setter);
        void Remove(IResultSetter<T> setter);
        void TrySetResult(T item);
    }
}