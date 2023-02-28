using StorageAccounting.Domain.Common;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace StorageAccounting.Infrastructure.Caching
{
    public interface ICacheService
    {
        Task RemoveMultiple(CancellationToken token = default, params string[] keys);

        Task<Result<T>> GetOrSetFromSource<T>(Func<Task<Result<T>>> source,
            string key,
            CancellationToken token = default,
            [CallerMemberName] string calleName = "");
    }
}
