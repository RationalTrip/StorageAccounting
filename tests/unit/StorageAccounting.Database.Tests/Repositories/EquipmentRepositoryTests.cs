using FluentAssertions;
using StorageAccounting.Database.Contexts;
using StorageAccounting.Database.Repositories;
using StorageAccounting.Database.Tests.ClassFixtures;
using StorageAccounting.Database.Tests.TestExtensions;
using StorageAccounting.Domain.Entities;
using StorageAccounting.Domain.Exceptions.Results;

namespace StorageAccounting.Database.Tests.Repositories
{
    public class EquipmentRepositoryTests : IClassFixture<StorageAccountingDbContextFixture>
    {
        private readonly StorageAccountingDbContext _context;
        public EquipmentRepositoryTests(StorageAccountingDbContextFixture contextFixture)
        {
            _context = contextFixture.Context;

            _context.ApplyData();
        }

        [Theory]
        [InlineData("Router", 5)]
        [InlineData("Cluster", 2)]
        [InlineData("Switch", 10)]
        public async Task CreateAsync_CreateExistedName_EntityExistsFaultResult(string existedName, int requiredArea)
        {
            //arrange
            var repo = new EquipmentRepository(_context);

            var existedEquipment = new Equipment
            {
                Name = existedName,
                RequiredArea = requiredArea
            };

            //act
            var actualResult = await repo.CreateAsync(existedEquipment, default);

            //assert
            actualResult.IsFaulted.Should().BeTrue();

            actualResult.Exception.Should().BeOfType<UniqueValueAlreadyExistsException>();
        }

        [Theory]
        [InlineData("Computer", 5)]
        [InlineData("Laptop", 2)]
        [InlineData("Keyboard", 10)]
        public async Task CreateAsync_ValidEntity_SuccessResult(string name, int requiredArea)
        {
            //arrange
            var repo = new EquipmentRepository(_context);

            var equipment = new Equipment
            {
                Name = name,
                RequiredArea = requiredArea
            };

            //act
            var actualResult = await repo.CreateAsync(equipment, default);

            //assert
            actualResult.IsSuccess.Should().BeTrue();

            actualResult.Value.Name.Should().Be(name);
            actualResult.Value.RequiredArea.Should().Be(requiredArea);

            _context.Equipments.Should().Contain(equipment);
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
            var repo = new EquipmentRepository(_context);

            var expectedResults = InitialData.GetInitialRentingContract
                .Where(contract => contract.EquipmentId == id)
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
            var repo = new EquipmentRepository(_context);

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
            var repo = new EquipmentRepository(_context);

            var expectedResults = InitialData.GetInitialRentingContract
                .Where(contract => contract.EquipmentId == id)
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
            var repo = new EquipmentRepository(_context);

            //act
            var actualResult = await repo.GetRentingContractsCountAsync(id, default);

            //assert
            actualResult.IsFaulted.Should().BeTrue();

            actualResult.Exception.Should().BeOfType<EntityNotFoundException>();
        }
    }
}
