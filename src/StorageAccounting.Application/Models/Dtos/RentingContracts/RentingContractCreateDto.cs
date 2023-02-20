using System;
using System.ComponentModel.DataAnnotations;

namespace StorageAccounting.Application.Models.Dtos.RentingContracts
{
    public class RentingContractCreateDto
    {
        [Required]
        public int RoomId { get; set; }
        [Required]
        public int EquipmentId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int EquipmentCount { get; set; }
    }
}
