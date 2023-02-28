using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using StorageAccounting.Application.Profiles;
using StorageAccounting.Application.Repositories;
using StorageAccounting.Application.Services;
using StorageAccounting.Database.Contexts;
using StorageAccounting.Database.Repositories;
using StorageAccounting.Infrastructure.Caching;
using StorageAccounting.Infrastructure.Caching.Decorators;
using StorageAccounting.Infrastructure.Services;
using StorageAccounting.WebAPI.Security.Authorization;

namespace StorageAccounting.WebAPI.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStorageAccountingDbContext(this IServiceCollection services,
            IConfiguration config) =>
            services.AddDbContext<StorageAccountingDbContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("StorageAccountingSqlServer"));
            });

        public static IServiceCollection AddStorageAccountingRedis(this IServiceCollection services,
            IConfiguration config) =>
            services.AddStackExchangeRedisCache(opt =>
            {
                opt.Configuration = config.GetConnectionString("StorageAccountingRedis");
                opt.InstanceName = "StorageAccounting.WebAPI";
            });

        public static IServiceCollection AddStorageAccountingAutoMapper(this IServiceCollection services) =>
            services.AddAutoMapper(opt =>
            {
                opt.AddProfile<EquipmentProfile>();
                opt.AddProfile<StorageRoomProfile>();
                opt.AddProfile<RentingContractProfile>();
            });

        public static IServiceCollection AddAuthorizationCustomResult(this IServiceCollection services) =>
            services.AddSingleton<IAuthorizationMiddlewareResultHandler, AuthorizationCustomResultMiddleware>();

        public static IServiceCollection AddRepositories(this IServiceCollection services) =>
            services.AddScoped<IEquipmentRepository, EquipmentRepository>()
                .AddScoped<IStorageRoomRepository, StorageRoomRepository>()
                .AddScoped<IRentingContractRepository, RentingContractRepository>();

        public static IServiceCollection AddStorageAccountingServices(this IServiceCollection services)
        {
            services.AddScoped<IEquipmentService, EquipmentService>();
            services.AddScoped<IStorageRoomService, StorageRoomService>();
            services.AddScoped<IRentingContractService, RentingContractService>();

            services.AddScoped<ICacheService, CacheService>();

            return services.AddServiceCachingDecorators();
        }

        static IServiceCollection AddServiceCachingDecorators(this IServiceCollection services) =>
            services.Decorate<IEquipmentService, EquipmentServiceCachingDecorator>()
                .Decorate<IStorageRoomService, StorageRoomServiceCachingDecorator>()
                .Decorate<IRentingContractService, RentingContractServiceCachingDecorator>();
    }
}
