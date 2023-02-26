using AutoMapper;
using StorageAccounting.Application.Profiles;

namespace StorageAccounting.Infrastructure.Tests.TestCommon
{
    internal static class AutoMapperSource
    {
        public static IMapper GetAutoMapper { get; } =
            new MapperConfiguration(conf => conf.AddMaps(typeof(EquipmentProfile).Assembly.FullName))
                .CreateMapper();
    }
}
