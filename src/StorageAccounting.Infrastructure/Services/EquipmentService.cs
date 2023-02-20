using AutoMapper;
using StorageAccounting.Application.Models.Dtos;
using StorageAccounting.Application.Models.Dtos.Equipments;
using StorageAccounting.Application.Models.Dtos.RentingContracts;
using StorageAccounting.Application.Repositories;
using StorageAccounting.Application.Services;
using StorageAccounting.Domain.Common;
using StorageAccounting.Domain.Entities;
using StorageAccounting.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StorageAccounting.Infrastructure.Services
{
    public sealed class EquipmentService : IEquipmentService
    {
        private readonly IEquipmentRepository _equipmentRepo;
        private readonly IMapper _mapper;

        public EquipmentService(IEquipmentRepository equipmentRepo,
            IMapper mapper)
        {
            _equipmentRepo = equipmentRepo;
            _mapper = mapper;
        }
        public async Task<Result<EquipmentReadDto>> CreateAsync(EquipmentCreateDto model,
            CancellationToken token)
        {
            var equipment = _mapper.Map<Equipment>(model);

            var creationResult = await _equipmentRepo.CreateAsync(equipment, token);

            if (creationResult.IsFaulted)
                return creationResult.AsFaultResult<EquipmentReadDto>();

            return _mapper.Map<EquipmentReadDto>(creationResult.Value);
        }

        public async Task<Result<IEnumerable<EquipmentReadDto>>> GetAllAsync(int? start,
            int? size,
            CancellationToken token)
        {
            var equipmentsResult = await _equipmentRepo.GetAllAsync(start, size, token);

            if (equipmentsResult.IsFaulted)
                return equipmentsResult.AsFaultResult<IEnumerable<EquipmentReadDto>>();

            return _mapper.Map<IEnumerable<EquipmentReadDto>>(equipmentsResult.Value)
                .AsResult();
        }

        public async Task<Result<CountReadDto>> GetCountAsync(CancellationToken token)
        {
            var count = await _equipmentRepo.GetCountAsync(token);

            return new(count);
        }

        public async Task<Result<EquipmentReadDto>> GetByIdAsync(int id, CancellationToken token)
        {
            var equipmentResult = await _equipmentRepo.GetById(id, token);

            if (equipmentResult.IsFaulted)
                return equipmentResult.AsFaultResult<EquipmentReadDto>();

            return _mapper.Map<EquipmentReadDto>(equipmentResult.Value);
        }

        public async Task<Result<IEnumerable<RentingContractReadDto>>> GetRentingContractsAsync(int equipmentId,
            int? start,
            int? size,
            CancellationToken token)
        {
            var contractsResult = await _equipmentRepo.GetRentingContractsAsync(equipmentId, start, size, token);

            if (contractsResult.IsFaulted)
                return contractsResult.AsFaultResult<IEnumerable<RentingContractReadDto>>();

            return _mapper.Map<IEnumerable<RentingContractReadDto>>(contractsResult.Value)
                .AsResult();
        }

        public async Task<Result<CountReadDto>> GetRentingContractsCountAsync(int equipmentId, CancellationToken token)
        {
            var countResult = await _equipmentRepo.GetRentingContractsCountAsync(equipmentId, token);

            if (countResult.IsFaulted)
                return countResult.AsFaultResult<CountReadDto>();

            return new(countResult.Value);
        }
    }
}
