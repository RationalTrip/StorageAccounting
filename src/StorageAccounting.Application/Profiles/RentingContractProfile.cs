using AutoMapper;
using StorageAccounting.Application.Models.Dtos.RentingContracts;
using StorageAccounting.Domain.Entities;

namespace StorageAccounting.Application.Profiles
{
    public class RentingContractProfile : Profile
    {
        public RentingContractProfile()
        {
            CreateMap<RentingContract, RentingContractReadDto>();
            CreateMap<RentingContractCreateDto, RentingContract>();
        }
    }
}