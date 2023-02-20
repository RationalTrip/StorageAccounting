namespace StorageAccounting.Application.Models.Dtos.StorageRooms
{
    public class StorageRoomReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TotalArea { get; set; }
    }
}
