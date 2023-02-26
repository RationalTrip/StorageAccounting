using StorageAccounting.Domain.Common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StorageAccounting.Application.Repositories
{
    public interface IRepository<T, TKey> where T : class
    {
        Task<int> GetCountAsync(CancellationToken token);
        Task<Result<IEnumerable<T>>> GetAllAsync(int? start, int? size, CancellationToken token);
        Task<Result<T>> GetByIdAsync(TKey id, CancellationToken token);
        Task<Result<T>> CreateAsync(T entity, CancellationToken token);
        Task<Result<T>> UpdateAsync(T entity, CancellationToken token);
        Task<bool> IsExistsAsync(TKey id, CancellationToken token);
        Task<Result> RemoveAsync(TKey id, CancellationToken token);
    }
}
