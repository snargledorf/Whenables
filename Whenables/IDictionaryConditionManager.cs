namespace Whenables
{
    public interface IDictionaryConditionManager<TKey, TValue>
    {
        void Add(IDictionaryCondition<TKey, TValue> dictionaryCondition);
        void Remove(IDictionaryCondition<TKey, TValue> dictionaryCondition);
        void SetKeyAndValueOnConditions(TKey key, TValue value);
    }
}