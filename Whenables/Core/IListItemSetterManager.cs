namespace Whenables.Core
{
    public interface IListItemSetterManager<T>
    {
        void Add(IListResultSetter<T> listResultSetter);
        void Remove(IListResultSetter<T> listResultSetter);
        void TrySet(T item, int index);
    }
}