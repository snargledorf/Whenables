using System;
using System.Threading;
using System.Threading.Tasks;

namespace Whenables.Core
{
    public interface IWhenable<T>
    {
        Task<T> WhenAsync(Func<T, bool> condition);
        Task<T> WhenAsync(Func<T, bool> condition, CancellationToken cancellationToken);
    }
}