using System;
using System.ComponentModel.DataAnnotations;

namespace StorageAccounting.Application.Models.Dtos.StorageRooms
{
    public class StorageRoomCreateDto
    {
        public string Name { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int TotalArea { get; set; }
    }
}
