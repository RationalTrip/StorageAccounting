using StorageAccounting.Application.Repositories;
using StorageAccounting.Database.Contexts;
using StorageAccounting.Domain.Entities;
using System;
using System.Linq.Expressions;

namespace StorageAccounting.Database.Repositories
{
    public sealed class RentingContractRepository : Repository<RentingContract, int>,
        IRentingContractRepository,
        IRepository<RentingContract, int>
    {
        public RentingContractRepository(StorageAccountingDbContext context) : base(context) { }

        protected override Expression<Func<RentingContract, int>> PrimaryKey =>
            contract => contract.Id;
    }
}
