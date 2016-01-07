using System.Collections.Generic;
using System.Linq;

namespace Whenables
{
    public class DictionaryConditionManager<TKey, TValue> : IDictionaryConditionManager<TKey, TValue>
    {
        private readonly HashSet<IDictionaryCondition<TKey, TValue>> conditions = new HashSet<IDictionaryCondition<TKey, TValue>>();

        public void Add(IDictionaryCondition<TKey, TValue> dictionaryCondition)
        {
            conditions.Add(dictionaryCondition);
        }

        public void Remove(IDictionaryCondition<TKey, TValue> dictionaryCondition)
        {
            conditions.Remove(dictionaryCondition);
        }

        public void SetKeyAndValueOnConditions(TKey key, TValue value)
        {
            foreach (IDictionaryCondition<TKey, TValue> condition in conditions.ToArray())
            {
                if (condition.TrySetKeyAndValue(key, value))
                    conditions.Remove(condition);
            }
        }
    }
}