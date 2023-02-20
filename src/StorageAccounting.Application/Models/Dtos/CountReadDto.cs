namespace StorageAccounting.Application.Models.Dtos
{
    public class CountReadDto
    {
        public int Count { get; set; }
        public static implicit operator CountReadDto(int count) => new CountReadDto { Count = count };
    }
}
