using StorageAccounting.Application.Models;
using StorageAccounting.Domain.Common;
using StorageAccounting.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StorageAccounting.Application.Repositories
{
    public interface IStorageRoomRepository : IRepository<StorageRoom, int>
    {
        Task<Result<IEnumerable<RentingContract>>> GetRentingContractsAsync(int storageRoomId,
            int? start,
            int? size,
            CancellationToken token);
        Task<Result<int>> GetRentingContractsCountAsync(int storageRoomId, CancellationToken token);
        Task<Result<StorageRoomRentedArea>> GetRentedAreaAsync(int id, CancellationToken token);
    }
}