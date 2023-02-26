using StorageAccounting.Domain.Common;
using StorageAccounting.Domain.Exceptions.Results;
using System;

namespace StorageAccounting.Infrastructure.Commons
{
    internal class CommonResults
    {
        public static Result<T> NotEnoughAreaResult<T>(int requiredArea, int avaliableArea) =>
            new Result<T>(new NotEnoughAreaException(requiredArea, avaliableArea));
    }
}
