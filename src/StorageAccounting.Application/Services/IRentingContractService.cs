using StorageAccounting.Application.Models.Dtos;
using StorageAccounting.Application.Models.Dtos.RentingContracts;
using StorageAccounting.Domain.Common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StorageAccounting.Application.Services
{
    public interface IRentingContractService
    {
        Task<Result<IEnumerable<RentingContractReadDto>>> GetAllAsync(int? start,
            int? size,
            CancellationToken token);

        Task<Result<CountReadDto>> GetCountAsync(CancellationToken token);

        Task<Result<RentingContractReadDto>> GetByIdAsync(int id, CancellationToken token);

        Task<Result<RentingContractReadDto>> CreateAsync(RentingContractCreateDto model,
            CancellationToken token);

        Task<Result> RemoveAsync(int id, CancellationToken token);
    }
}
