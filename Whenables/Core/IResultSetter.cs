namespace Whenables.Core
{
    public interface IResultSetter<in T>
    {
        bool TrySetResult(T result);
    }
}