namespace Whenables
{
    public interface IConditionManager<T>
    {
        void Add(ICondition<T> condition);
        void Remove(ICondition<T> condition);
        void TrySetItemOnConditions(T item);
    }
}