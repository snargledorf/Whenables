namespace Whenables
{
    public interface IListConditionManager<T>
    {
        void Add(IListCondition<T> listCondition);
        void Remove(IListCondition<T> listCondition);
        void TrySetItemOnConditions(T item, int index);
    }
}