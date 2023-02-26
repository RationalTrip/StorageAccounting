using FluentAssertions;
using StorageAccounting.Application.Models;
using StorageAccounting.Database.Contexts;
using StorageAccounting.Database.Repositories;
using StorageAccounting.Database.Tests.ClassFixtures;
using StorageAccounting.Database.Tests.TestExtensions;
using StorageAccounting.Domain.Entities;
using StorageAccounting.Domain.Exceptions.Results;

namespace StorageAccounting.Database.Tests.Repositories
{
    public class StorageRoomRepositoryTests : IClassFixture<StorageAccountingDbContextFixture>
    {
        private readonly StorageAccountingDbContext _context;
        public StorageRoomRepositoryTests(StorageAccountingDbContextFixture contextFixture)
        {
            _context = contextFixture.Context;

            _context.ApplyData();
        }

        [Theory]
        [InlineData("Dorm: Lomonosova 35", 25)]
        [InlineData("Office: Kyiv, Polyva 21", 12)]
        [InlineData("Storage: general", 15)]
        public async Task CreateAsync_CreateExistedName_EntityExistsFaultResult(string existedName, int totalArea)
        {
            //arrange
            var repo = new StorageRoomRepository(_context);

            var existedRoom = new StorageRoom
            {
                Name = existedName,
                TotalArea = totalArea
            };

            //act
            var actualResult = await repo.CreateAsync(existedRoom, default);

            //assert
            actualResult.IsFaulted.Should().BeTrue();

            actualResult.Exception.Should().BeOfType<UniqueValueAlreadyExistsException>();
        }

        [Theory]
        [InlineData("Storage: str. Nezaleznosti", 51)]
        [InlineData("Market: Varus", 22)]
        [InlineData("Market: ATB", 1287)]
        public async Task CreateAsync_ValidEntity_SuccessResult(string name, int totalArea)
        {
            //arrange
            var repo = new StorageRoomRepository(_context);

            var room = new StorageRoom
            {
                Name = name,
                TotalArea = totalArea
            };

            //act
            var actualResult = await repo.CreateAsync(room, default);

            //assert
            actualResult.IsSuccess.Should().BeTrue();

            actualResult.Value.Name.Should().Be(name);
            actualResult.Value.TotalArea.Should().Be(totalArea);

            _context.StorageRooms.Should().Contain(room);
        }

        [Theory]
        [InlineData(1, null, 5)]
        [InlineData(2, 1, 2)]
        [InlineData(3, 2, null)]
        [InlineData(4)]
        [InlineData(5)]
        public async Task GetRentingContractsAsync_ExistedId_SuccessResult(int id,
            int? start = null,
            int? size = null)
        {
            //arrange
            var repo = new StorageRoomRepository(_context);

            var expectedResults = InitialData.GetInitialRentingContract
                .Where(contract => contract.RoomId == id)
                .OrderBy(contract => contract.Id)
                .AsEnumerable();

            if (start is not null)
                expectedResults = expectedResults.Skip(start.Value);

            if (size is not null)
                expectedResults = expectedResults.Take(size.Value);

            //act
            var actualResult = await repo.GetRentingContractsAsync(id, start, size, default);

            //assert
            actualResult.IsSuccess.Should().BeTrue();

            actualResult.Value.Select(contract => contract.Id)
                .Should()
                .BeEquivalentTo(expectedResults.Select(contract => contract.Id));
        }

        [Theory]
        [InlineData(12, null, 5)]
        [InlineData(2352, 1, 2)]
        [InlineData(-4, 2, null)]
        [InlineData(13)]
        [InlineData(1738)]
        public async Task GetRentingContractsAsync_NotExistedId_NotFoundResult(int id,
            int? start = null,
            int? size = null)
        {
            //arrange
            var repo = new StorageRoomRepository(_context);

            //act
            var actualResult = await repo.GetRentingContractsAsync(id, start, size, default);

            //assert
            actualResult.IsFaulted.Should().BeTrue();

            actualResult.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public async Task GetRentingContractsCountAsync_ExistedId_SuccessResult(int id)
        {
            //arrange
            var repo = new StorageRoomRepository(_context);

            var expectedResults = InitialData.GetInitialRentingContract
                .Where(contract => contract.RoomId == id)
                .Count();

            //act
            var actualResult = await repo.GetRentingContractsCountAsync(id, default);

            //assert
            actualResult.IsSuccess.Should().BeTrue();

            actualResult.Value.Should().Be(expectedResults);
        }

        [Theory]
        [InlineData(12)]
        [InlineData(2352)]
        [InlineData(-4)]
        [InlineData(13)]
        [InlineData(1738)]
        public async Task GetRentingContractsCountAsync_NotExistedId_NotFoundResult(int id)
        {
            //arrange
            var repo = new StorageRoomRepository(_context);

            //act
            var actualResult = await repo.GetRentingContractsCountAsync(id, default);

            //assert
            actualResult.IsFaulted.Should().BeTrue();

            actualResult.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Theory]
        [InlineData(1, 10, 8)]
        [InlineData(2, 250, 150)]
        [InlineData(3, 1000, 300)]
        [InlineData(4, 800, 500)]
        [InlineData(5, 50, 35)]
        [InlineData(6, 100, 0)]
        public async Task GetRentedAreaAsync_ExistedId_SuccessResult(int id, int totalArea, int rentedArea)
        {
            //arrange
            var repo = new StorageRoomRepository(_context);

            var expectedResult = new StorageRoomRentedArea
            {
                Id = id,
                TotalArea = totalArea,
                RentedArea = rentedArea
            };

            //act
            var actualResult = await repo.GetRentedAreaAsync(id, default);

            //assert
            actualResult.IsSuccess.Should().BeTrue();

            actualResult.Value.Should().BeEquivalentTo(expectedResult);
        }

        [Theory]
        [InlineData(113)]
        [InlineData(2122)]
        [InlineData(-5)]
        [InlineData(10)]
        [InlineData(11238)]
        public async Task GetRentedAreaAsync_NotExistedId_NotFoundResult(int id)
        {
            //arrange
            var repo = new StorageRoomRepository(_context);

            //act
            var actualResult = await repo.GetRentedAreaAsync(id, default);

            //assert
            actualResult.IsFaulted.Should().BeTrue();

            actualResult.Exception.Should().BeOfType<EntityNotFoundException>();
        }
    }
}
