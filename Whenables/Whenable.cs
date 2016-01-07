using System;

namespace Whenables
{
    public class Whenable<T> : IWhenable<T>
    {
        private readonly ConditionManager<T> manager = new ConditionManager<T>();

        private T value;

        public Whenable()
        {
        }

        public Whenable(T value)
        {
            this.value = value;
        }

        public T Value
        {
            get { return value; }
            set
            {
                this.value = value;
                manager.TrySetItemOnConditions(value);
            }
        }

        public ICondition<T> When(Func<T, bool> condition)
        {
            var c = new Condition<T>(condition);
            manager.Add(c);
            return c;
        }

        public static implicit operator T(Whenable<T> source)
        {
            return source.Value;
        }
    }
}
