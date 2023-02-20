using Microsoft.EntityFrameworkCore;
using StorageAccounting.Application.Models;
using StorageAccounting.Application.Repositories;
using StorageAccounting.Database.Contexts;
using StorageAccounting.Database.Extensions;
using StorageAccounting.Domain.Common;
using StorageAccounting.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace StorageAccounting.Database.Repositories
{
    public sealed class StorageRoomRepository : Repository<StorageRoom, int>,
        IStorageRoomRepository,
        IRepository<StorageRoom, int>
    {
        private readonly StorageAccountingDbContext _context;
        public StorageRoomRepository(StorageAccountingDbContext context) : base(context) =>
            _context = context;

        protected override Expression<Func<StorageRoom, int>> PrimaryKey =>
            room => room.Id;

        public override async Task<Result<StorageRoom>> CreateAsync(StorageRoom entity, CancellationToken token)
        {
            var existedRoom = await _context.StorageRooms
                    .AsNoTracking()
                    .FirstOrDefaultAsync(room => room.Name == entity.Name);

            if (existedRoom is not null)
                return UniqueValueAlreadyExistsResult(existedRoom.Name,
                    nameof(StorageRoom.Name),
                    existedRoom.Id.ToString());

            return await base.CreateAsync(entity, token);
        }

        public async Task<Result<StorageRoomRentedArea>> GetRentedAreaAsync(int id, CancellationToken token)
        {
            var roomArea = await GetRentedAreaQuery(id)
                .AsNoTracking()
                .FirstOrDefaultAsync(token);

            if (roomArea is null)
                return EntityNotFoundResult(id)
                    .AsFaultResult<StorageRoomRentedArea>();

            return roomArea;
        }

        private IQueryable<StorageRoomRentedArea> GetRentedAreaQuery(int id) =>
            from room in _context.StorageRooms
            where room.Id == id
            select new StorageRoomRentedArea
            {
                Id = room.Id,
                TotalArea = room.TotalArea,
                RentedArea = room.Contracts
                                    .Sum(contract => contract.EquipmentCount * contract.Equipment.RequiredArea)
            };
        public async Task<Result<IEnumerable<RentingContract>>> GetRentingContractsAsync(int storageRoomId,
            int? start,
            int? size,
            CancellationToken token)
        {
            if(!await IsExistsAsync(storageRoomId, token))
                return EntityNotFoundResult(storageRoomId)
                    .AsFaultResult<IEnumerable<RentingContract>>();

            return await _context.RentingContracts
                    .Where(contract => contract.RoomId == storageRoomId)
                    .OptionalPagination(contract => contract.Id, start, size)
                    .AsNoTracking()
                    .ToListAsync(token);
        }
        public async Task<Result<int>> GetRentingContractsCountAsync(int storageRoomId, CancellationToken token)
        {
            var count = await _context.StorageRooms
                .Where(room => room.Id == storageRoomId)
                .Select(equipment => new { Count = equipment.Contracts.Count })
                .FirstOrDefaultAsync(token);

            if (count is null)
                return EntityNotFoundResult(storageRoomId)
                    .AsFaultResult<int>();

            return count.Count;
        }
    }
}
