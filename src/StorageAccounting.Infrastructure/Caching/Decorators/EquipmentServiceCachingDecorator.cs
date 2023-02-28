using StorageAccounting.Application.Models.Dtos;
using StorageAccounting.Application.Models.Dtos.Equipments;
using StorageAccounting.Application.Models.Dtos.RentingContracts;
using StorageAccounting.Application.Services;
using StorageAccounting.Domain.Common;
using StorageAccounting.Infrastructure.Caching.Statics;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StorageAccounting.Infrastructure.Caching.Decorators
{
    public class EquipmentServiceCachingDecorator : IEquipmentService
    {
        private readonly ICacheService _cacheService;
        private readonly IEquipmentService _service;

        public EquipmentServiceCachingDecorator(ICacheService cacheService, IEquipmentService service)
        {
            _cacheService = cacheService;
            _service = service;
        }

        public async Task<Result<EquipmentReadDto>> CreateAsync(EquipmentCreateDto model, CancellationToken token)
        {
            var result = await _service.CreateAsync(model, token);

            if (result.IsFaulted)
                return result;

            await _cacheService.RemoveMultiple(token,
                EquipmentKeys.GetAllKey(),
                EquipmentKeys.GetCountKey());

            return result;
        }

        public async Task<Result<IEnumerable<EquipmentReadDto>>> GetAllAsync(int? start,
            int? size,
            CancellationToken token)
        {
            if (start is not null || size is not null)
                return await _service.GetAllAsync(start, size, token);

            return await _cacheService.GetOrSetFromSource(
                    () => _service.GetAllAsync(start, size, token),
                    EquipmentKeys.GetAllKey(),
                    token);
        }

        public async Task<Result<EquipmentReadDto>> GetByIdAsync(int id, CancellationToken token) =>
            await _cacheService.GetOrSetFromSource(
                        () => _service.GetByIdAsync(id, token),
                        EquipmentKeys.GetByIdKey(id),
                        token);

        public async Task<Result<CountReadDto>> GetCountAsync(CancellationToken token) =>
            await _cacheService.GetOrSetFromSource(
                        () => _service.GetCountAsync(token),
                        EquipmentKeys.GetCountKey(),
                        token);

        //can not be cached because we can not track which contracts was removed
        public async Task<Result<IEnumerable<RentingContractReadDto>>> GetRentingContractsAsync(int equipmentId,
            int? start,
            int? size,
            CancellationToken token) =>
            await _service.GetRentingContractsAsync(equipmentId, start, size, token);

        //can not be cached because we can not track which contracts was removed
        public async Task<Result<CountReadDto>> GetRentingContractsCountAsync(int equipmentId,
            CancellationToken token) =>
            await _service.GetRentingContractsCountAsync(equipmentId, token);
    }
}
