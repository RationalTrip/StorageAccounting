using System.Collections.Generic;

namespace StorageAccounting.Domain.Entities
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int RequiredArea { get; set; }
        public List<RentingContract> Contracts { get; set; } = new List<RentingContract>();
    }
}
