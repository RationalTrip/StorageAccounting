using Microsoft.AspNetCore.Mvc;
using StorageAccounting.Application.Models.Dtos;
using StorageAccounting.Domain.Exceptions.Results;

namespace StorageAccounting.WebAPI.Extensions
{
    internal static class ExceptionHandlingExtensions
    {
        public static ActionResult Handle(this Exception exc) => exc switch
        {
            EntityNotFoundException notFoundExc =>
                new NotFoundObjectResult(notFoundExc.ToErrorDto(StatusCodes.Status404NotFound)),

            NotEnoughArea notEnoughAreaExc =>
                new BadRequestObjectResult(notEnoughAreaExc.ToErrorDto(StatusCodes.Status400BadRequest)),

            UniqueValueAlreadyExistsException valueExistsExc =>
                new BadRequestObjectResult(valueExistsExc.ToErrorDto(StatusCodes.Status400BadRequest)),

            _ => throw exc
        };

        public static object ToErrorDto(this StorageAccountingException exc, int statusCode) =>
            new ErrorDto
            {
                StatusCode = statusCode,
                Title = exc.Title,
                Message = exc.Message
            };
    }
}
