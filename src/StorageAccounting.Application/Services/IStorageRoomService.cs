using StorageAccounting.Application.Models.Dtos;
using StorageAccounting.Application.Models.Dtos.RentingContracts;
using StorageAccounting.Application.Models.Dtos.StorageRooms;
using StorageAccounting.Domain.Common;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StorageAccounting.Application.Services
{
    public interface IStorageRoomService
    {
        Task<Result<IEnumerable<StorageRoomReadDto>>> GetAllAsync(int? start,
            int? size,
            CancellationToken token);

        Task<Result<CountReadDto>> GetCountAsync(CancellationToken token);

        Task<Result<IEnumerable<RentingContractReadDto>>> GetRentingContractsAsync(int roomId,
            int? start,
            int? size,
            CancellationToken token);

        Task<Result<CountReadDto>> GetRentingContractsCountAsync(int roomId,
            CancellationToken token);

        Task<Result<StorageRoomReadDto>> GetByIdAsync(int id, CancellationToken token);
        Task<Result<StorageRoomReadDto>> CreateAsync(StorageRoomCreateDto model, CancellationToken token);
        Task<Result<StorageRoomRentedAreaReadDto>> GetRentedAreaAsync(int roomId, CancellationToken token);
    }
}
