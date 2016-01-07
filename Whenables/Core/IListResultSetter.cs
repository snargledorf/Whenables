namespace Whenables.Core
{
    public interface IListResultSetter<in T>
    {
        bool TrySetResult(T result, int index);
    }
}