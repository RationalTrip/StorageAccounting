using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StorageAccounting.Domain.Entities;

namespace StorageAccounting.Database.Contexts.Configuration
{
    internal class RentingContractConfiguration : IEntityTypeConfiguration<RentingContract>
    {
        public void Configure(EntityTypeBuilder<RentingContract> builder)
        {
            builder.HasKey(contract => contract.Id);
            builder.Property(contract => contract.Id)
                .UseIdentityColumn(seed: 50);

            builder.Property(contract => contract.EquipmentId)
                .IsRequired();

            builder.Property(contract => contract.RoomId)
                .IsRequired();

            builder.Property(contract => contract.EquipmentCount)
                .IsRequired();

            builder.HasOne(contract => contract.Equipment)
                .WithMany(equipment => equipment.Contracts)
                .HasForeignKey(contract => contract.EquipmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(contract => contract.Room)
                .WithMany(room => room.Contracts)
                .HasForeignKey(contract => contract.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(InitialData.GetInitialRentingContract);
        }
    }
}
