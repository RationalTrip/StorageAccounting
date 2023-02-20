using StorageAccounting.Domain.Common;
using StorageAccounting.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StorageAccounting.Application.Repositories
{
    public interface IEquipmentRepository : IRepository<Equipment, int>
    {
        Task<Result<IEnumerable<RentingContract>>> GetRentingContractsAsync(int equipmentId,
            int? start,
            int? size,
            CancellationToken token);

        Task<Result<int>> GetRentingContractsCountAsync(int equipmentId, CancellationToken token);
    }
}