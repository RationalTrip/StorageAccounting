using AutoMapper;
using StorageAccounting.Application.Models.Dtos;
using StorageAccounting.Application.Models.Dtos.RentingContracts;
using StorageAccounting.Application.Repositories;
using StorageAccounting.Application.Services;
using StorageAccounting.Domain.Common;
using StorageAccounting.Domain.Entities;
using StorageAccounting.Infrastructure.Commons;
using StorageAccounting.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StorageAccounting.Infrastructure.Services
{
    public sealed class RentingContractService : IRentingContractService
    {
        private readonly IRentingContractRepository _contractRepo;
        private readonly IStorageRoomRepository _roomRepo;
        private readonly IEquipmentRepository _equipmentRepo;
        private readonly IMapper _mapper;

        public RentingContractService(IRentingContractRepository contractRepo,
            IStorageRoomRepository roomRepo,
            IEquipmentRepository equipmentRepo,
            IMapper mapper)
        {
            _contractRepo = contractRepo;
            _roomRepo = roomRepo;
            _equipmentRepo = equipmentRepo;
            _mapper = mapper;
        }
        public async Task<Result<RentingContractReadDto>> CreateAsync(RentingContractCreateDto model,
            CancellationToken token)
        {
            var contract = _mapper.Map<RentingContract>(model);

            var validationResult = await ValidateContractAsync(contract, token);
            if (validationResult.IsFaulted)
                return validationResult.AsFaultResult<RentingContractReadDto>();

            var creationResult = await _contractRepo.CreateAsync(contract, token);

            if (creationResult.IsFaulted)
                return creationResult.AsFaultResult<RentingContractReadDto>();

            return _mapper.Map<RentingContractReadDto>(creationResult.Value);
        }

        private async Task<Result<RentingContract>> ValidateContractAsync(RentingContract contract,
            CancellationToken token)
        {
            var equipmentResult = await _equipmentRepo.GetByIdAsync(contract.EquipmentId, token);
            if (equipmentResult.IsFaulted)
                return equipmentResult.AsFaultResult<RentingContract>();

            var rentedAreaResult = await _roomRepo.GetRentedAreaAsync(contract.RoomId, token);
            if (rentedAreaResult.IsFaulted)
                return rentedAreaResult.AsFaultResult<RentingContract>();

            var rentedArea = rentedAreaResult.Value;

            var avaliableArea = rentedArea.TotalArea - rentedArea.RentedArea;
            var requiredArea = contract.EquipmentCount * equipmentResult.Value.RequiredArea;

            if (requiredArea > avaliableArea)
                return CommonResults.NotEnoughAreaResult<RentingContract>(requiredArea, avaliableArea);

            return contract;
        }

        public async Task<Result<IEnumerable<RentingContractReadDto>>> GetAllAsync(int? start,
            int? size,
            CancellationToken token)
        {
            var contractsResult = await _contractRepo.GetAllAsync(start, size, token);

            if (contractsResult.IsFaulted)
                return contractsResult.AsFaultResult<IEnumerable<RentingContractReadDto>>();

            return _mapper.Map<IEnumerable<RentingContractReadDto>>(contractsResult.Value)
                .AsResult();
        }

        public async Task<Result<CountReadDto>> GetCountAsync(CancellationToken token)
        {
            var count = await _contractRepo.GetCountAsync(token);

            return new(count);
        }

        public async Task<Result<RentingContractReadDto>> GetByIdAsync(int id, CancellationToken token)
        {
            var contractResult = await _contractRepo.GetByIdAsync(id, token);

            if (contractResult.IsFaulted)
                return contractResult.AsFaultResult<RentingContractReadDto>();

            return _mapper.Map<RentingContractReadDto>(contractResult.Value);
        }

        public async Task<Result> RemoveAsync(int id, CancellationToken token) =>
            await _contractRepo.RemoveAsync(id, token);

    }
}
