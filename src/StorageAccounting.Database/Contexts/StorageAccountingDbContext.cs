using Microsoft.EntityFrameworkCore;
using StorageAccounting.Database.Contexts.Configuration;
using StorageAccounting.Domain.Entities;

namespace StorageAccounting.Database.Contexts
{
    public sealed class StorageAccountingDbContext : DbContext
    {
        public StorageAccountingDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<RentingContract> RentingContracts { get; set; }
        public DbSet<StorageRoom> StorageRooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EquipmentConfiguration());
            modelBuilder.ApplyConfiguration(new RentingContractConfiguration());
            modelBuilder.ApplyConfiguration(new StorageRoomConfiguration());
        }
    }
}
