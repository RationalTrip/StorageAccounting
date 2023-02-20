namespace StorageAccounting.Application.Models.Dtos
{
    public class ErrorDto
    {
        public int StatusCode { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
