namespace StorageAccounting.Domain.Entities
{
    public class RentingContract
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public StorageRoom Room { get; set; } = default!;
        public int EquipmentId { get; set; }
        public Equipment Equipment { get; set; } = default!;
        public int EquipmentCount { get; set; }
    }
}