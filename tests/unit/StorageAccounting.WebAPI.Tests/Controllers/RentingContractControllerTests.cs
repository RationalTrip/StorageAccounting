using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using StorageAccounting.Application.Models.Dtos;
using StorageAccounting.Application.Models.Dtos.RentingContracts;
using StorageAccounting.Application.Services;
using StorageAccounting.Domain.Common;
using StorageAccounting.WebAPI.Controllers;
using StorageAccounting.WebAPI.Tests.CommonTests;
using StorageAccounting.WebAPI.Tests.TestExtensions;

namespace StorageAccounting.WebAPI.Tests.Controllers
{
    public class RentingContractControllerTests
    {
        private Mock<IRentingContractService> contractServiceMock;
        public RentingContractControllerTests()
        {
            contractServiceMock = new Mock<IRentingContractService>();
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(1)]
        [InlineData(null, 1)]
        [InlineData(1, 1)]
        public async Task GetAllAsync_Success_SuccessResult(int? start = null, int? size = null)
        {
            //arrange
            var expectedResultValue = new RentingContractReadDto[] { new RentingContractReadDto() };

            contractServiceMock
                .Setup(service => service.GetAllAsync(start, size, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<IEnumerable<RentingContractReadDto>>(expectedResultValue));


            var controller = new RentingContractController(contractServiceMock.Object);

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

            contractServiceMock
                .Setup(service => service.GetCountAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<CountReadDto>(expectedResultValue));

            var controller = new RentingContractController(contractServiceMock.Object);

            //act
            var actualResult = await controller.GetCountAsync(default);

            //assign
            actualResult.Result.Should()
                .BeObjectResult(StatusCodes.Status200OK, expectedResultValue);
        }

        [Theory]
        [InlineData(1, 1, 1, 1)]
        [InlineData(5, 22, 7, 213)]
        public async Task GetByIdAsync_ExistedId_SuccessResult(int id,
            int roomId,
            int equipmentId,
            int equipmentCount)
        {
            //arrange
            var expectedResultValue = new RentingContractReadDto
            {
                Id = id,
                RoomId = roomId,
                EquipmentId = equipmentId,
                EquipmentCount = equipmentCount
            };

            contractServiceMock
                .Setup(service => service.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<RentingContractReadDto>(expectedResultValue));

            var controller = new RentingContractController(contractServiceMock.Object);

            //act
            var actualResult = await controller.GetByIdAsync(id, default);

            //assign
            actualResult.Result.Should()
                .BeObjectResult(StatusCodes.Status200OK, expectedResultValue);
        }

        [Theory]
        [MemberData(nameof(ControllerTestInputs.EntityNotFoundTestInput),
            "17",
            "RentingContract",
            MemberType = typeof(ControllerTestInputs))]
        public async Task GetByIdAsync_ErrorServiceResponse_ErrorResult(Exception exc,
            int expectedStatusCode,
            ErrorDto expectedResult)
        {
            //arrange
            var id = 17;

            contractServiceMock
                .Setup(service => service.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<RentingContractReadDto>(exc));

            var controller = new RentingContractController(contractServiceMock.Object);

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
            var intputModel = new RentingContractCreateDto
            {
                RoomId = 32,
                EquipmentId = 17,
                EquipmentCount = 123
            };

            var expectedResultValue = new RentingContractReadDto
            {
                Id = 17,
                RoomId = intputModel.RoomId,
                EquipmentId = intputModel.EquipmentId,
                EquipmentCount = intputModel.EquipmentCount
            };

            contractServiceMock
                .Setup(service => service.CreateAsync(intputModel, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<RentingContractReadDto>(expectedResultValue));

            var controller = new RentingContractController(contractServiceMock.Object);

            //act
            var actualResult = await controller.CreateAsync(intputModel, default);

            //assign
            actualResult.Result.Should()
                .BeObjectResult(StatusCodes.Status201Created, expectedResultValue);
        }

        [Theory]
        [MemberData(nameof(ControllerTestInputs.EntityNotFoundTestInput),
            "32",
            "StorageRoom",
            MemberType = typeof(ControllerTestInputs))]
        [MemberData(nameof(ControllerTestInputs.EntityNotFoundTestInput),
            "17",
            "Equipment",
            MemberType = typeof(ControllerTestInputs))]
        [MemberData(nameof(ControllerTestInputs.NotEnoughAreaTestInput),
            175,
            180,
            MemberType = typeof(ControllerTestInputs))]
        public async Task CreateAsync_ErrorServiceResponse_ErrorResult(Exception exc,
            int expectedStatusCode,
            ErrorDto expectedResult)
        {
            //arrange
            var intputModel = new RentingContractCreateDto
            {
                RoomId = 32,
                EquipmentId = 17,
                EquipmentCount = 123
            };

            contractServiceMock
                .Setup(service => service.CreateAsync(intputModel, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result<RentingContractReadDto>(exc));

            var controller = new RentingContractController(contractServiceMock.Object);

            //act
            var actualResult = await controller.CreateAsync(intputModel, default);

            //assign
            actualResult.Result.Should()
                .BeObjectResult(expectedStatusCode, expectedResult);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(25)]
        [InlineData(12)]
        public async Task RemoveAsync_ExistedId_SuccessResult(int id)
        {
            //arrange
            contractServiceMock
                .Setup(service => service.RemoveAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result());

            var controller = new RentingContractController(contractServiceMock.Object);

            //act
            var actualResult = await controller.RemoveAsync(id, default);

            //assign
            actualResult.Should()
                .BeStatusCodeResult(StatusCodes.Status200OK);
        }

        [Theory]
        [MemberData(nameof(ControllerTestInputs.EntityNotFoundTestInput),
            "17",
            "RentingContract",
            MemberType = typeof(ControllerTestInputs))]
        public async Task GetRentingContractsAsync_ErrorServiceResponse_ErrorResult(Exception exc,
            int expectedStatusCode,
            ErrorDto expectedResult)
        {
            //arrange
            var id = 17;

            contractServiceMock
                .Setup(service => service.RemoveAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Result(exc));

            var controller = new RentingContractController(contractServiceMock.Object);

            //act
            var actualResult = await controller.RemoveAsync(id, default);

            //assign
            actualResult.Should()
                .BeObjectResult(expectedStatusCode, expectedResult);
        }
    }
}
