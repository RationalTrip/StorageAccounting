using StorageAccounting.Application.Models.Dtos;
using StorageAccounting.Application.Models.Dtos.RentingContracts;
using StorageAccounting.Application.Services;
using StorageAccounting.Domain.Common;
using StorageAccounting.Infrastructure.Caching.Statics;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StorageAccounting.Infrastructure.Caching.Decorators
{
    public class RentingContractServiceCachingDecorator : IRentingContractService
    {
        private readonly ICacheService _cacheService;
        private readonly IRentingContractService _service;

        public RentingContractServiceCachingDecorator(IRentingContractService service, ICacheService cacheService)
        {
            _service = service;
            _cacheService = cacheService;
        }

        public async Task<Result<RentingContractReadDto>> CreateAsync(RentingContractCreateDto model,
            CancellationToken token)
        {
            var result = await _service.CreateAsync(model, token);

            if (result.IsFaulted)
                return result;

            await _cacheService.RemoveMultiple(token,
                RentingContractKeys.GetAllKey(),
                RentingContractKeys.GetCountKey());

            return result;
        }

        public async Task<Result<IEnumerable<RentingContractReadDto>>> GetAllAsync(int? start,
            int? size,
            CancellationToken token)
        {
            if (start is not null || size is not null)
                return await _service.GetAllAsync(start, size, token);

            return await _cacheService.GetOrSetFromSource(
                    () => _service.GetAllAsync(start, size, token),
                    RentingContractKeys.GetAllKey(),
                    token);
        }

        public async Task<Result<RentingContractReadDto>> GetByIdAsync(int id, CancellationToken token) =>
            await _cacheService.GetOrSetFromSource(
                        () => _service.GetByIdAsync(id, token),
                        RentingContractKeys.GetByIdKey(id),
                        token);

        public async Task<Result<CountReadDto>> GetCountAsync(CancellationToken token) =>
            await _cacheService.GetOrSetFromSource(
                        () => _service.GetCountAsync(token),
                        RentingContractKeys.GetCountKey(),
                        token);

        public async Task<Result> RemoveAsync(int id, CancellationToken token)
        {
            var result = await _service.RemoveAsync(id, token);

            if (result.IsFaulted)
                return result;

            await _cacheService.RemoveMultiple(token,
                RentingContractKeys.GetAllKey(),
                RentingContractKeys.GetCountKey(),
                RentingContractKeys.GetByIdKey(id));

            return result;
        }
    }
}
