using FluentAssertions;
using Moq;
using StorageAccounting.Application.Models;
using StorageAccounting.Application.Models.Dtos.RentingContracts;
using StorageAccounting.Application.Repositories;
using StorageAccounting.Domain.Common;
using StorageAccounting.Domain.Entities;
using StorageAccounting.Domain.Exceptions.Results;
using StorageAccounting.Infrastructure.Services;
using StorageAccounting.Infrastructure.Tests.TestCommon;

namespace StorageAccounting.Infrastructure.Tests.Services
{
    public class RentingContractServiceTests
    {
        private Mock<IRentingContractRepository> contractRepoMock;
        private Mock<IStorageRoomRepository> roomRepoMock;
        private Mock<IEquipmentRepository> equipmentRepoMock;
        public RentingContractServiceTests()
        {
            contractRepoMock = new Mock<IRentingContractRepository>();
            roomRepoMock = new Mock<IStorageRoomRepository>();
            equipmentRepoMock = new Mock<IEquipmentRepository>();
        }

        [Theory]
        [MemberData(nameof(CreateAsyncSuccessInput))]
        public async Task CreateAsync_ValidEntity_SuccessResult(RentingContractCreateDto input,
            Equipment equipmentRepoRes,
            StorageRoomRentedArea rentedAreaRepoRes,
            RentingContract createdContractRepoRes)
        {
            //arrange
            equipmentRepoMock
                .Setup(repo => repo.GetByIdAsync(input.EquipmentId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(equipmentRepoRes);

            roomRepoMock
                .Setup(repo => repo.GetRentedAreaAsync(input.RoomId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(rentedAreaRepoRes);

            contractRepoMock
                .Setup(repo => repo.CreateAsync(It.IsAny<RentingContract>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdContractRepoRes);

            var mapper = AutoMapperSource.GetAutoMapper;

            var expectedResultValue = mapper.Map<RentingContractReadDto>(createdContractRepoRes);

            var contractService = new RentingContractService(contractRepoMock.Object,
                roomRepoMock.Object,
                equipmentRepoMock.Object,
                mapper);

            //act
            var actualResult = await contractService.CreateAsync(input, default);

            //assert
            actualResult.IsSuccess.Should().BeTrue();

            actualResult.Value.Should().BeEquivalentTo(expectedResultValue);
        }

        [Theory]
        [MemberData(nameof(CreateAsyncNotEnoughtAreaInput))]
        public async Task CreateAsync_NotEnoughArea_FaultResult(RentingContractCreateDto input,
            Equipment equipmentRepoRes,
            StorageRoomRentedArea rentedAreaRepoRes)
        {
            //arrange
            equipmentRepoMock
                .Setup(repo => repo.GetByIdAsync(input.EquipmentId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(equipmentRepoRes);

            roomRepoMock
                .Setup(repo => repo.GetRentedAreaAsync(input.RoomId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(rentedAreaRepoRes);

            var contractService = new RentingContractService(contractRepoMock.Object,
                roomRepoMock.Object,
                equipmentRepoMock.Object,
                AutoMapperSource.GetAutoMapper);

            //act
            var actualResult = await contractService.CreateAsync(input, default);

            //assert
            actualResult.IsFaulted.Should().BeTrue();

            actualResult.Exception.Should().BeOfType<NotEnoughAreaException>();
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(12, 1131, 1321)]
        [InlineData(42, 13, 1321)]
        public async Task CreateAsync_NotExistedEquipment_FaultResult(int equipmentId,
            int roomId,
            int equipmentCount)
        {
            //arrange
            var input = new RentingContractCreateDto
            {
                RoomId = roomId,
                EquipmentId = equipmentId,
                EquipmentCount = equipmentCount
            };

            equipmentRepoMock
                .Setup(repo => repo.GetByIdAsync(input.EquipmentId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    new Result<Equipment>(new EntityNotFoundException(input.EquipmentId.ToString()))
                    );

            roomRepoMock
                .Setup(repo => repo.GetRentedAreaAsync(input.RoomId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new StorageRoomRentedArea());

            var contractService = new RentingContractService(contractRepoMock.Object,
                roomRepoMock.Object,
                equipmentRepoMock.Object,
                AutoMapperSource.GetAutoMapper);

            //act
            var actualResult = await contractService.CreateAsync(input, default);

            //assert
            actualResult.IsFaulted.Should().BeTrue();

            actualResult.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(2, 1234, 11)]
        [InlineData(14, 157, 1321)]
        public async Task CreateAsync_NotExistedRoom_FaultResult(int equipmentId,
            int roomId,
            int equipmentCount)
        {
            //arrange
            var input = new RentingContractCreateDto
            {
                RoomId = roomId,
                EquipmentId = equipmentId,
                EquipmentCount = equipmentCount
            };

            equipmentRepoMock
                .Setup(repo => repo.GetByIdAsync(input.EquipmentId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Equipment());

            roomRepoMock
                .Setup(repo => repo.GetRentedAreaAsync(input.RoomId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    new Result<StorageRoomRentedArea>(new EntityNotFoundException(input.RoomId.ToString()))
                    );

            var contractService = new RentingContractService(contractRepoMock.Object,
                roomRepoMock.Object,
                equipmentRepoMock.Object,
                AutoMapperSource.GetAutoMapper);

            //act
            var actualResult = await contractService.CreateAsync(input, default);

            //assert
            actualResult.IsFaulted.Should().BeTrue();

            actualResult.Exception.Should().BeOfType<EntityNotFoundException>();
        }

        public static IEnumerable<object[]> CreateAsyncSuccessInput =>
            new object[][]
            {
                new object[]
                {
                    new RentingContractCreateDto
                    {
                        EquipmentId = 2,
                        RoomId = 13,
                        EquipmentCount = 5
                    },
                    new Equipment
                    {
                        Id = 2,
                        RequiredArea = 5
                    },
                    new StorageRoomRentedArea
                    {
                        Id = 13,
                        RentedArea = 56,
                        TotalArea = 100
                    },
                    new RentingContract
                    {
                        Id = 35,
                        EquipmentId = 2,
                        RoomId = 13,
                        EquipmentCount = 5
                    }
                },
                new object[]
                {
                    new RentingContractCreateDto
                    {
                        EquipmentId = 45,
                        RoomId = 25,
                        EquipmentCount = 10
                    },
                    new Equipment
                    {
                        Id = 45,
                        RequiredArea = 10
                    },
                    new StorageRoomRentedArea
                    {
                        Id = 25,
                        RentedArea = 100,
                        TotalArea = 200
                    },
                    new RentingContract
                    {
                        Id = 436,
                        EquipmentId = 45,
                        RoomId = 25,
                        EquipmentCount = 10
                    }
                },
                new object[]
                {
                    new RentingContractCreateDto
                    {
                        EquipmentId = 265,
                        RoomId = 2438,
                        EquipmentCount = 21
                    },
                    new Equipment
                    {
                        Id = 265,
                        RequiredArea = 23
                    },
                    new StorageRoomRentedArea
                    {
                        Id = 2438,
                        RentedArea = 2335,
                        TotalArea = 1252131
                    },
                    new RentingContract
                    {
                        Id = 13251,
                        EquipmentId = 265,
                        RoomId = 2438,
                        EquipmentCount = 21
                    }
                }
            };

        public static IEnumerable<object[]> CreateAsyncNotEnoughtAreaInput =>
            new object[][]
            {
                new object[]
                {
                    new RentingContractCreateDto
                    {
                        EquipmentId = 2,
                        RoomId = 13,
                        EquipmentCount = 5
                    },
                    new Equipment
                    {
                        Id = 2,
                        RequiredArea = 5
                    },
                    new StorageRoomRentedArea
                    {
                        Id = 13,
                        RentedArea = 89,
                        TotalArea = 100
                    }
                },
                new object[]
                {
                    new RentingContractCreateDto
                    {
                        EquipmentId = 45,
                        RoomId = 25,
                        EquipmentCount = 10
                    },
                    new Equipment
                    {
                        Id = 45,
                        RequiredArea = 10
                    },
                    new StorageRoomRentedArea
                    {
                        Id = 25,
                        RentedArea = 100,
                        TotalArea = 199
                    }
                },
                new object[]
                {
                    new RentingContractCreateDto
                    {
                        EquipmentId = 265,
                        RoomId = 2438,
                        EquipmentCount = 21
                    },
                    new Equipment
                    {
                        Id = 265,
                        RequiredArea = 23
                    },
                    new StorageRoomRentedArea
                    {
                        Id = 2438,
                        RentedArea = 2335,
                        TotalArea = 2335
                    }
                }
            };
    }
}
