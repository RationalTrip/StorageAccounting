namespace StorageAccounting.Application.Models.Dtos.RentingContracts
{
    public class RentingContractReadDto
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int EquipmentId { get; set; }
        public int EquipmentCount { get; set; }
    }
}
