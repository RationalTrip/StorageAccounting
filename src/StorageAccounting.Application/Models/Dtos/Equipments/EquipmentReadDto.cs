using System;

namespace StorageAccounting.Application.Models.Dtos.Equipments
{
    public class EquipmentReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int RequiredArea { get; set; }
    }
}
