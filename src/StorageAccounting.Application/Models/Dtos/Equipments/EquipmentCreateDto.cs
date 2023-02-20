using System;
using System.ComponentModel.DataAnnotations;

namespace StorageAccounting.Application.Models.Dtos.Equipments
{
    public class EquipmentCreateDto
    {
        public string Name { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int RequiredArea { get; set; }
    }
}
