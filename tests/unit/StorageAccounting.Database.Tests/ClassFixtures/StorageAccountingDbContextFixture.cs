using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using StorageAccounting.Database.Contexts;

namespace StorageAccounting.Database.Tests.ClassFixtures
{
    public class StorageAccountingDbContextFixture : IDisposable
    {
        private StorageAccountingDbContext _context;

        public StorageAccountingDbContextFixture()
        {
            string id = string.Format("{0}.db", Guid.NewGuid().ToString());

            var connectionStringBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = id,
                Mode = SqliteOpenMode.Memory,
                Cache = SqliteCacheMode.Shared
            };

            var optionBuilder = new DbContextOptionsBuilder()
                .UseSqlite(connectionStringBuilder.ConnectionString)
                .EnableSensitiveDataLogging();
            _context = new StorageAccountingDbContext(optionBuilder.Options);

            _context.Database.OpenConnection();
        }

        public StorageAccountingDbContext Context => _context;

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Database.CloseConnection();
        }
    }
}
