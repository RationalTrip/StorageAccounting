using AutoMapper;
using StorageAccounting.Application.Models;
using StorageAccounting.Application.Models.Dtos.StorageRooms;
using StorageAccounting.Domain.Entities;

namespace StorageAccounting.Application.Profiles
{
    public class StorageRoomProfile : Profile
    {
        public StorageRoomProfile()
        {
            CreateMap<StorageRoomCreateDto, StorageRoom>();
            CreateMap<StorageRoom, StorageRoomReadDto>();

            CreateMap<StorageRoomRentedArea, StorageRoomRentedAreaReadDto>();
        }
    }
}