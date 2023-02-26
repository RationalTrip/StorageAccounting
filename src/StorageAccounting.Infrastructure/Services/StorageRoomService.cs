using AutoMapper;
using StorageAccounting.Application.Models.Dtos;
using StorageAccounting.Application.Models.Dtos.RentingContracts;
using StorageAccounting.Application.Models.Dtos.StorageRooms;
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
    public sealed class StorageRoomService : IStorageRoomService
    {
        private readonly IStorageRoomRepository _roomRepo;
        private readonly IMapper _mapper;

        public StorageRoomService(IStorageRoomRepository roomRepo, IMapper mapper)
        {
            _roomRepo = roomRepo;
            _mapper = mapper;
        }
        public async Task<Result<StorageRoomReadDto>> CreateAsync(StorageRoomCreateDto model,
            CancellationToken token)
        {
            var room = _mapper.Map<StorageRoom>(model);

            var createResult = await _roomRepo.CreateAsync(room, token);

            if (createResult.IsFaulted)
                return createResult.AsFaultResult<StorageRoomReadDto>();

            return _mapper.Map<StorageRoomReadDto>(createResult.Value);
        }

        public async Task<Result<IEnumerable<StorageRoomReadDto>>> GetAllAsync(int? start,
            int? size,
            CancellationToken token)
        {
            var roomsResult = await _roomRepo.GetAllAsync(start, size, token);

            if (roomsResult.IsFaulted)
                return roomsResult.AsFaultResult<IEnumerable<StorageRoomReadDto>>();

            return _mapper.Map<IEnumerable<StorageRoomReadDto>>(roomsResult.Value)
                .AsResult();
        }

        public async Task<Result<CountReadDto>> GetCountAsync(CancellationToken token)
        {
            var count = await _roomRepo.GetCountAsync(token);

            return new(count);
        }

        public async Task<Result<StorageRoomReadDto>> GetByIdAsync(int id, CancellationToken token)
        {
            var roomResult = await _roomRepo.GetByIdAsync(id, token);

            if (roomResult.IsFaulted)
                return roomResult.AsFaultResult<StorageRoomReadDto>();

            return _mapper.Map<StorageRoomReadDto>(roomResult.Value);
        }

        public async Task<Result<StorageRoomRentedAreaReadDto>> GetRentedAreaAsync(int roomId, CancellationToken token)
        {
            var rentedAreaResult = await _roomRepo.GetRentedAreaAsync(roomId, token);

            if (rentedAreaResult.IsFaulted)
                return rentedAreaResult.AsFaultResult<StorageRoomRentedAreaReadDto>();

            return _mapper.Map<StorageRoomRentedAreaReadDto>(rentedAreaResult.Value);
        }

        public async Task<Result<IEnumerable<RentingContractReadDto>>> GetRentingContractsAsync(int roomId,
            int? start,
            int? size,
            CancellationToken token)
        {
            var contractsResult = await _roomRepo.GetRentingContractsAsync(roomId, start, size, token);

            if (contractsResult.IsFaulted)
                return contractsResult.AsFaultResult<IEnumerable<RentingContractReadDto>>();

            return _mapper.Map<IEnumerable<RentingContractReadDto>>(contractsResult.Value)
                .AsResult();
        }

        public async Task<Result<CountReadDto>> GetRentingContractsCountAsync(int roomId, CancellationToken token)
        {
            var countResult = await _roomRepo.GetRentingContractsCountAsync(roomId, token);

            if (countResult.IsFaulted)
                return countResult.AsFaultResult<CountReadDto>();

            return new(countResult.Value);
        }
    }
}
