using StorageAccounting.Application.Models.Dtos;
using StorageAccounting.Application.Models.Dtos.RentingContracts;
using StorageAccounting.Application.Models.Dtos.StorageRooms;
using StorageAccounting.Application.Services;
using StorageAccounting.Domain.Common;
using StorageAccounting.Infrastructure.Caching.Statics;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StorageAccounting.Infrastructure.Caching.Decorators
{
    public class StorageRoomServiceCachingDecorator : IStorageRoomService
    {
        private readonly ICacheService _cacheService;
        private readonly IStorageRoomService _service;

        public StorageRoomServiceCachingDecorator(IStorageRoomService service, ICacheService cacheService)
        {
            _service = service;
            _cacheService = cacheService;
        }

        public async Task<Result<StorageRoomReadDto>> CreateAsync(StorageRoomCreateDto model, CancellationToken token)
        {
            var result = await _service.CreateAsync(model, token);

            if (result.IsFaulted)
                return result;

            await _cacheService.RemoveMultiple(token,
                StorageRoomKeys.GetAllKey(),
                StorageRoomKeys.GetCountKey());

            return result;
        }

        public async Task<Result<IEnumerable<StorageRoomReadDto>>> GetAllAsync(int? start, int? size, CancellationToken token)
        {
            if (start is not null || size is not null)
                return await _service.GetAllAsync(start, size, token);

            return await _cacheService.GetOrSetFromSource(
                    () => _service.GetAllAsync(start, size, token),
                    StorageRoomKeys.GetAllKey(),
                    token);
        }

        public async Task<Result<StorageRoomReadDto>> GetByIdAsync(int id, CancellationToken token) =>
            await _cacheService.GetOrSetFromSource(
                        () => _service.GetByIdAsync(id, token),
                        StorageRoomKeys.GetByIdKey(id),
                        token);

        public async Task<Result<CountReadDto>> GetCountAsync(CancellationToken token) =>
            await _cacheService.GetOrSetFromSource(
                        () => _service.GetCountAsync(token),
                        StorageRoomKeys.GetCountKey(),
                        token);

        //can not be cached because we can not track which contracts was removed
        public async Task<Result<StorageRoomRentedAreaReadDto>> GetRentedAreaAsync(int roomId, CancellationToken token) =>
            await _service.GetRentedAreaAsync(roomId, token);

        //can not be cached because we can not track which contracts was removed
        public async Task<Result<IEnumerable<RentingContractReadDto>>> GetRentingContractsAsync(int roomId,
            int? start,
            int? size,
            CancellationToken token) =>
            await _service.GetRentingContractsAsync(roomId, start, size, token);

        //can not be cached because we can not track which contracts was removed
        public async Task<Result<CountReadDto>> GetRentingContractsCountAsync(int roomId, CancellationToken token) =>
            await _service.GetRentingContractsCountAsync(roomId, token);
    }
}
