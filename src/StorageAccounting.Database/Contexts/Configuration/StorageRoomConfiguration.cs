using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StorageAccounting.Domain.Entities;

namespace StorageAccounting.Database.Contexts.Configuration
{
    internal class StorageRoomConfiguration : IEntityTypeConfiguration<StorageRoom>
    {
        public void Configure(EntityTypeBuilder<StorageRoom> builder)
        {
            builder.HasKey(room => room.Id);
            builder.Property(room => room.Id)
                .UseIdentityColumn(seed: 10);

            builder.Property(room => room.Name)
                .HasMaxLength(256)
                .IsRequired();
            builder.HasIndex(room => room.Name)
                .IsUnique();

            builder.Property(room => room.TotalArea)
                .IsRequired();

            builder.HasMany(room => room.Contracts)
                .WithOne(contract => contract.Room)
                .HasForeignKey(contract => contract.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(InitialData.GetInitialStorageRooms);
        }
    }
}
