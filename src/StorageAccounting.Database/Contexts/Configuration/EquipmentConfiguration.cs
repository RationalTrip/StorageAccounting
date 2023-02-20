using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StorageAccounting.Domain.Entities;

namespace StorageAccounting.Database.Contexts.Configuration
{
    internal class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
    {
        public void Configure(EntityTypeBuilder<Equipment> builder)
        {
            builder.HasKey(equipment => equipment.Id);
            builder.Property(equipment => equipment.Id)
                .UseIdentityColumn(seed: 10);

            builder.Property(equipment => equipment.Name)
                .HasMaxLength(256)
                .IsRequired();
            builder.HasIndex(equipment => equipment.Name)
                .IsUnique();

            builder.Property(equipment => equipment.RequiredArea)
                .IsRequired();

            builder.HasMany(equipment => equipment.Contracts)
                .WithOne(contract => contract.Equipment)
                .HasForeignKey(contract => contract.EquipmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(InitialData.GetInitialEquipment);
        }
    }
}
