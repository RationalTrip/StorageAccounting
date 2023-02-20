using AutoMapper;
using StorageAccounting.Application.Models.Dtos.Equipments;
using StorageAccounting.Domain.Entities;

namespace StorageAccounting.Application.Profiles
{
    public class EquipmentProfile : Profile
    {
        public EquipmentProfile()
        {
            CreateMap<EquipmentCreateDto, Equipment>();
            CreateMap<Equipment, EquipmentReadDto>();
        }
    }
}