using Microsoft.AspNetCore.Http;
using StorageAccounting.Application.Models.Dtos;
using StorageAccounting.Domain.Exceptions.Results;

namespace StorageAccounting.WebAPI.Tests.CommonTests
{
    public static class ControllerTestInputs
    {
        public static IEnumerable<object[]> EntityNotFoundTestInput(string id, string entityType)
        {
            var exception = new EntityNotFoundException(id, entityType);

            return GetErrorTestInput(exception, StatusCodes.Status404NotFound);
        }

        public static IEnumerable<object[]> NotEnoughAreaTestInput(int rentedArea, int availableArea)
        {
            var exception = new NotEnoughAreaException(rentedArea, availableArea);

            return GetErrorTestInput(exception, StatusCodes.Status400BadRequest);
        }

        public static IEnumerable<object[]> UniqueValueExistsTestInput(string value,
            string propertyName,
            string entityName,
            string existedEntityId)
        {
            var exception = new UniqueValueAlreadyExistsException(value,
                propertyName,
                entityName,
                existedEntityId);

            return GetErrorTestInput(exception, StatusCodes.Status400BadRequest);
        }

        private static IEnumerable<object[]> GetErrorTestInput(StorageAccountingException exc, int statusCode)
        {
            var expectedResult = new ErrorDto
            {
                StatusCode = statusCode,
                Title = exc.Title,
                Message = exc.Message
            };

            return new object[][]
            {
                new object[]{ exc, statusCode, expectedResult}
            };
        }
    }
}
