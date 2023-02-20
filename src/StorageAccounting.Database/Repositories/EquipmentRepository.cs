using Microsoft.EntityFrameworkCore;
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
    public sealed class EquipmentRepository : Repository<Equipment, int>,
        IEquipmentRepository,
        IRepository<Equipment, int>
    {
        private readonly StorageAccountingDbContext _context;
        public EquipmentRepository(StorageAccountingDbContext context) : base(context) =>
            _context = context;

        protected override Expression<Func<Equipment, int>> PrimaryKey =>
            equipment => equipment.Id;

        public override async Task<Result<Equipment>> CreateAsync(Equipment entity, CancellationToken token)
        {
            var existedEquipment = await _context.Equipments
                .AsNoTracking()
                .FirstOrDefaultAsync(equipment => equipment.Name == entity.Name, token);

            if (existedEquipment is not null)
                return UniqueValueAlreadyExistsResult(existedEquipment.Name,
                    nameof(Equipment.Name),
                    existedEquipment.Id.ToString());

            return await base.CreateAsync(entity, token);
        }

        public async Task<Result<IEnumerable<RentingContract>>> GetRentingContractsAsync(int equipmentId,
            int? start,
            int? size,
            CancellationToken token)
        {
            if(!await IsExistsAsync(equipmentId, token))
                return EntityNotFoundResult(equipmentId)
                    .AsFaultResult<IEnumerable<RentingContract>>();

            return await _context.RentingContracts
                    .Where(contract => contract.EquipmentId == equipmentId)
                    .OptionalPagination(contract => contract.Id, start, size)
                    .AsNoTracking()
                    .ToListAsync(token);
        }
            

        public async Task<Result<int>> GetRentingContractsCountAsync(int equipmentId, CancellationToken token)
        {
            var count = await _context.Equipments
                .Where(equipment => equipment.Id == equipmentId)
                .Select(equipment => new { Count = equipment.Contracts.Count })
                .FirstOrDefaultAsync(token);

            if (count is null)
                return EntityNotFoundResult(equipmentId)
                    .AsFaultResult<int>();

            return count.Count;
        }
    }
}
