using System.Collections.Generic;

namespace StorageAccounting.Domain.Entities
{
    public class StorageRoom
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TotalArea { get; set; }
        public List<RentingContract> Contracts { get; set; } = new List<RentingContract>();
    }
}