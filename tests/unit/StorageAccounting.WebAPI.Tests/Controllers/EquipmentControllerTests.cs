using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using StorageAccounting.Application.Models.Dtos;
using StorageAccounting.Application.Models.Dtos.Equipments;
using StorageAccounting.Application.Models.Dtos.RentingContracts;
using StorageAccounting.Application.Services;
using StorageAccounting.Domain.Common;
using StorageAccounting.WebAPI.Controllers;
using StorageAccounting.WebAPI.Tests.CommonTests;
using StorageAccounting.WebAPI.Tests.TestExtensions;

namespace StorageAccounting.WebAPI.Tests.Controllers
{
    public class EquipmentControllerTests
    {
        private Mock<IEquipmentService> equipmentServiceMock;
        public EquipmentControllerTests()
        {
            equipmentServiceMock = new Mock<IEquipmentService>();
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(1)]
        [InlineData(null, 1)]
        [InlineData(1, 1)]
        public async Task GetAllAsync_Success_SuccessResult(int? start = null, int? size = null)
        {
            //arrange
            var expectedResultValue = new EquipmentReadDto[] { new EquipmentReadDto() };

            equipmentServiceMock
                .Setup(service => service.GetAllAsync(start, size, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<IEnumerable<EquipmentReadDto>>(expectedResultValue));


            var controller = new EquipmentController(equipmentServiceMock.Object);

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

            equipmentServiceMock
                .Setup(service => service.GetCountAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<CountReadDto>(expectedResultValue));

            var controller = new EquipmentController(equipmentServiceMock.Object);

            //act
            var actualResult = await controller.GetCountAsync(default);

            //assign
            actualResult.Result.Should()
                .BeObjectResult(StatusCodes.Status200OK, expectedResultValue);
        }

        [Theory]
        [InlineData(1, "Some name", 1)]
        [InlineData(5, "Another name", 15)]
        public async Task GetByIdAsync_ExistedId_SuccessResult(int id, string equipmentName, int requiredArea)
        {
            //arrange
            var expectedResultValue = new EquipmentReadDto
            {
                Id = id,
                Name = equipmentName,
                RequiredArea = requiredArea
            };

            equipmentServiceMock
                .Setup(service => service.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<EquipmentReadDto>(expectedResultValue));

            var controller = new EquipmentController(equipmentServiceMock.Object);

            //act
            var actualResult = await controller.GetByIdAsync(id, default);

            //assign
            actualResult.Result.Should()
                .BeObjectResult(StatusCodes.Status200OK, expectedResultValue);
        }

        [Theory]
        [MemberData(nameof(ControllerTestInputs.EntityNotFoundTestInput),
            "17",
            "Equipment",
            MemberType = typeof(ControllerTestInputs))]
        public async Task GetByIdAsync_ErrorServiceResponse_ErrorResult(Exception exc,
            int expectedStatusCode,
            ErrorDto expectedResult)
        {
            //arrange
            var id = 17;

            equipmentServiceMock
                .Setup(service => service.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<EquipmentReadDto>(exc));

            var controller = new EquipmentController(equipmentServiceMock.Object);

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
            var intputModel = new EquipmentCreateDto
            {
                Name = "Some name",
                RequiredArea = 15
            };

            var expectedResultValue = new EquipmentReadDto
            {
                Id = 17,
                Name = intputModel.Name,
                RequiredArea = intputModel.RequiredArea
            };

            equipmentServiceMock
                .Setup(service => service.CreateAsync(intputModel, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<EquipmentReadDto>(expectedResultValue));

            var controller = new EquipmentController(equipmentServiceMock.Object);

            //act
            var actualResult = await controller.CreateAsync(intputModel, default);

            //assign
            actualResult.Result.Should()
                .BeObjectResult(StatusCodes.Status201Created, expectedResultValue);
        }

        [Theory]
        [MemberData(nameof(ControllerTestInputs.UniqueValueExistsTestInput),
            "Router",
            "Name",
            "Equipment",
            "1",
            MemberType = typeof(ControllerTestInputs))]
        public async Task CreateAsync_ErrorServiceResponse_ErrorResult(Exception exc,
            int expectedStatusCode,
            ErrorDto expectedResult)
        {
            //arrange
            var intputModel = new EquipmentCreateDto
            {
                Name = "Router",
                RequiredArea = 15
            };

            equipmentServiceMock
                .Setup(service => service.CreateAsync(intputModel, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<EquipmentReadDto>(exc));

            var controller = new EquipmentController(equipmentServiceMock.Object);

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

            equipmentServiceMock
                .Setup(service => service.GetRentingContractsAsync(id, start, size, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<IEnumerable<RentingContractReadDto>>(expectedResultValue));

            var controller = new EquipmentController(equipmentServiceMock.Object);

            //act
            var actualResult = await controller.GetRentingContractsAsync(id, default, start, size);

            //assign
            actualResult.Result.Should()
                .BeObjectResult(StatusCodes.Status200OK, expectedResultValue);
        }

        [Theory]
        [MemberData(nameof(ControllerTestInputs.EntityNotFoundTestInput),
            "17",
            "Equipment",
            MemberType = typeof(ControllerTestInputs))]
        public async Task GetRentingContractsAsync_ErrorServiceResponse_ErrorResult(Exception exc,
            int expectedStatusCode,
            ErrorDto expectedResult)
        {
            //arrange
            var id = 17;

            equipmentServiceMock
                .Setup(service => service.GetRentingContractsAsync(id, null, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<IEnumerable<RentingContractReadDto>>(exc));

            var controller = new EquipmentController(equipmentServiceMock.Object);

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

            equipmentServiceMock
                .Setup(service => service.GetRentingContractsCountAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<CountReadDto>(expectedResultValue));

            var controller = new EquipmentController(equipmentServiceMock.Object);

            //act
            var actualResult = await controller.GetRentingContractsCountAsync(id, default);

            //assign
            actualResult.Result.Should()
                .BeObjectResult(StatusCodes.Status200OK, expectedResultValue);
        }

        [Theory]
        [MemberData(nameof(ControllerTestInputs.EntityNotFoundTestInput),
            "17",
            "Equipment",
            MemberType = typeof(ControllerTestInputs))]
        public async Task GetRentingContractsCountAsync_ErrorServiceResponse_ErrorResult(Exception exc,
            int expectedStatusCode,
            ErrorDto expectedResult)
        {
            //arrange
            var id = 17;

            equipmentServiceMock
                .Setup(service => service.GetRentingContractsCountAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<CountReadDto>(exc));

            var controller = new EquipmentController(equipmentServiceMock.Object);

            //act
            var actualResult = await controller.GetRentingContractsCountAsync(id, default);

            //assign
            actualResult.Result.Should()
                .BeObjectResult(expectedStatusCode, expectedResult);
        }
    }
}
