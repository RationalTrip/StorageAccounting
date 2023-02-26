using StorageAccounting.Database.Contexts;

namespace StorageAccounting.Database.Tests.TestExtensions
{
    internal static class StorageAccountingDbContextExtensions
    {
        public static void ApplyData(this StorageAccountingDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
    }
}
