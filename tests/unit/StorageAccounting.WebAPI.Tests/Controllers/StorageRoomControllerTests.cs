using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using StorageAccounting.Application.Models.Dtos;
using StorageAccounting.Application.Models.Dtos.RentingContracts;
using StorageAccounting.Application.Models.Dtos.StorageRooms;
using StorageAccounting.Application.Services;
using StorageAccounting.Domain.Common;
using StorageAccounting.WebAPI.Controllers;
using StorageAccounting.WebAPI.Tests.CommonTests;
using StorageAccounting.WebAPI.Tests.TestExtensions;

namespace StorageAccounting.WebAPI.Tests.Controllers
{
    public class StorageRoomControllerTests
    {
        private Mock<IStorageRoomService> roomServiceMock;
        public StorageRoomControllerTests()
        {
            roomServiceMock = new Mock<IStorageRoomService>();
        }


        [Theory]
        [InlineData(null, null)]
        [InlineData(1)]
        [InlineData(null, 1)]
        [InlineData(1, 1)]
        public async Task GetAllAsync_Success_SuccessResult(int? start = null, int? size = null)
        {
            //arrange
            var expectedResultValue = new StorageRoomReadDto[] { new StorageRoomReadDto() };

            roomServiceMock
                .Setup(service => service.GetAllAsync(start, size, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<IEnumerable<StorageRoomReadDto>>(expectedResultValue));


            var controller = new StorageRoomController(roomServiceMock.Object);

            //act
            var actualResult = await controller.GetAllAsync(default, start, size);

            //assign
            actualResult.Result.Should()
                .BeObjectResult(StatusCodes.Status200OK, expectedResultValue);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        public async Task GetAllCountAsync_Success_SuccessResult(int resultCount)
        {
            //arrange
            var expectedResultValue = new CountReadDto
            {
                Count = resultCount
            };

            roomServiceMock
                .Setup(service => service.GetCountAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<CountReadDto>(expectedResultValue));

            var controller = new StorageRoomController(roomServiceMock.Object);

            //act
            var actualResult = await controller.GetCountAsync(default);

            //assign
            actualResult.Result.Should()
                .BeObjectResult(StatusCodes.Status200OK, expectedResultValue);
        }

        [Theory]
        [InlineData(1, "Some name", 100)]
        [InlineData(5, "Another name", 1214)]
        public async Task GetByIdAsync_ExistedId_SuccessResult(int id, string equipmentName, int totalArea)
        {
            //arrange
            var expectedResultValue = new StorageRoomReadDto
            {
                Id = id,
                Name = equipmentName,
                TotalArea = totalArea
            };

            roomServiceMock
                .Setup(service => service.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<StorageRoomReadDto>(expectedResultValue));

            var controller = new StorageRoomController(roomServiceMock.Object);

            //act
            var actualResult = await controller.GetByIdAsync(id, default);

            //assign
            actualResult.Result.Should()
                .BeObjectResult(StatusCodes.Status200OK, expectedResultValue);
        }

        [Theory]
        [MemberData(nameof(ControllerTestInputs.EntityNotFoundTestInput),
            "17",
            "StorageRoom",
            MemberType = typeof(ControllerTestInputs))]
        public async Task GetByIdAsync_ErrorServiceResponse_ErrorResult(Exception exc,
            int expectedStatusCode,
            ErrorDto expectedResult)
        {
            //arrange
            var id = 17;

            roomServiceMock
                .Setup(service => service.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<StorageRoomReadDto>(exc));

            var controller = new StorageRoomController(roomServiceMock.Object);

            //act
            var actualResult = await controller.GetByIdAsync(id, default);

            //assign
            actualResult.Result.Should()
                .BeObjectResult(expectedStatusCode, expectedResult);
        }

        [Fact]
        public async Task CreateAsync_ExistedId_SuccessResult()
        {
            //arrange
            var intputModel = new StorageRoomCreateDto
            {
                Name = "Some name",
                TotalArea = 150
            };

            var expectedResultValue = new StorageRoomReadDto
            {
                Id = 17,
                Name = intputModel.Name,
                TotalArea = intputModel.TotalArea
            };

            roomServiceMock
                .Setup(service => service.CreateAsync(intputModel, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<StorageRoomReadDto>(expectedResultValue));

            var controller = new StorageRoomController(roomServiceMock.Object);

            //act
            var actualResult = await controller.CreateAsync(intputModel, default);

            //assign
            actualResult.Result.Should()
                .BeObjectResult(StatusCodes.Status201Created, expectedResultValue);
        }

        [Theory]
        [MemberData(nameof(ControllerTestInputs.UniqueValueExistsTestInput),
            "Dorm",
            "Name",
            "StorageRoom",
            "150",
            MemberType = typeof(ControllerTestInputs))]
        public async Task CreateAsync_ErrorServiceResponse_ErrorResult(Exception exc,
            int expectedStatusCode,
            ErrorDto expectedResult)
        {
            //arrange
            var intputModel = new StorageRoomCreateDto
            {
                Name = "Dorm",
                TotalArea = 150
            };

            roomServiceMock
                .Setup(service => service.CreateAsync(intputModel, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<StorageRoomReadDto>(exc));

            var controller = new StorageRoomController(roomServiceMock.Object);

            //act
            var actualResult = await controller.CreateAsync(intputModel, default);

            //assign
            actualResult.Result.Should()
                .BeObjectResult(expectedStatusCode, expectedResult);
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(5, null, 15)]
        [InlineData(25, 15, null)]
        [InlineData(12, null, null)]
        public async Task GetRentingContractsAsync_ExistedId_SuccessResult(int id,
            int? start,
            int? size)
        {
            //arrange
            var expectedResultValue = new RentingContractReadDto[]
            {
                new RentingContractReadDto()
            };

            roomServiceMock
                .Setup(service => service.GetRentingContractsAsync(id, start, size, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<IEnumerable<RentingContractReadDto>>(expectedResultValue));

            var controller = new StorageRoomController(roomServiceMock.Object);

            //act
            var actualResult = await controller.GetRentingContractsAsync(id, default, start, size);

            //assign
            actualResult.Result.Should()
                .BeObjectResult(StatusCodes.Status200OK, expectedResultValue);
        }

        [Theory]
        [MemberData(nameof(ControllerTestInputs.EntityNotFoundTestInput),
            "17",
            "StorageRoom",
            MemberType = typeof(ControllerTestInputs))]
        public async Task GetRentingContractsAsync_ErrorServiceResponse_ErrorResult(Exception exc,
            int expectedStatusCode,
            ErrorDto expectedResult)
        {
            //arrange
            var id = 17;

            roomServiceMock
                .Setup(service => service.GetRentingContractsAsync(id, null, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<IEnumerable<RentingContractReadDto>>(exc));

            var controller = new StorageRoomController(roomServiceMock.Object);

            //act
            var actualResult = await controller.GetRentingContractsAsync(id, default, null, null);

            //assign
            actualResult.Result.Should()
                .BeObjectResult(expectedStatusCode, expectedResult);
        }

        [Theory]
        [InlineData(2, 12)]
        [InlineData(23, 5)]
        public async Task GetRentingContractsCountAsync_Success_SuccessResult(int id, int resultCount)
        {
            //arrange
            var expectedResultValue = new CountReadDto
            {
                Count = resultCount
            };

            roomServiceMock
                .Setup(service => service.GetRentingContractsCountAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<CountReadDto>(expectedResultValue));

            var controller = new StorageRoomController(roomServiceMock.Object);

            //act
            var actualResult = await controller.GetRentingContractsCountAsync(id, default);

            //assign
            actualResult.Result.Should()
                .BeObjectResult(StatusCodes.Status200OK, expectedResultValue);
        }

        [Theory]
        [MemberData(nameof(ControllerTestInputs.EntityNotFoundTestInput),
            "17",
            "StorageRoom",
            MemberType = typeof(ControllerTestInputs))]
        public async Task GetRentingContractsCountAsync_ErrorServiceResponse_ErrorResult(Exception exc,
            int expectedStatusCode,
            ErrorDto expectedResult)
        {
            //arrange
            var id = 17;

            roomServiceMock
                .Setup(service => service.GetRentingContractsCountAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<CountReadDto>(exc));

            var controller = new StorageRoomController(roomServiceMock.Object);

            //act
            var actualResult = await controller.GetRentingContractsCountAsync(id, default);

            //assign
            actualResult.Result.Should()
                .BeObjectResult(expectedStatusCode, expectedResult);
        }

        [Theory]
        [InlineData(2, 120, 150)]
        [InlineData(23, 127, 127)]
        public async Task GetStorageRoomRentedAreaAsync_Success_SuccessResult(int id,
            int rentedArea,
            int totalArea)
        {
            //arrange
            var expectedResultValue = new StorageRoomRentedAreaReadDto
            {
                Id = id,
                RentedArea = rentedArea,
                TotalArea = totalArea
            };

            roomServiceMock
                .Setup(service => service.GetRentedAreaAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<StorageRoomRentedAreaReadDto>(expectedResultValue));

            var controller = new StorageRoomController(roomServiceMock.Object);

            //act
            var actualResult = await controller.GetStorageRoomRentedAreaAsync(id, default);

            //assign
            actualResult.Result.Should()
                .BeObjectResult(StatusCodes.Status200OK, expectedResultValue);
        }

        [Theory]
        [MemberData(nameof(ControllerTestInputs.EntityNotFoundTestInput),
            "17",
            "StorageRoom",
            MemberType = typeof(ControllerTestInputs))]
        public async Task GetStorageRoomRentedAreaAsync_ErrorServiceResponse_ErrorResult(Exception exc,
            int expectedStatusCode,
            ErrorDto expectedResult)
        {
            //arrange
            var id = 17;

            roomServiceMock
                .Setup(service => service.GetRentedAreaAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<StorageRoomRentedAreaReadDto>(exc));

            var controller = new StorageRoomController(roomServiceMock.Object);

            //act
            var actualResult = await controller.GetStorageRoomRentedAreaAsync(id, default);

            //assign
            actualResult.Result.Should()
                .BeObjectResult(expectedStatusCode, expectedResult);
        }
    }
}
