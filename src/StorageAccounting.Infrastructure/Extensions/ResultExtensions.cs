using StorageAccounting.Domain.Common;

namespace StorageAccounting.Infrastructure.Extensions
{
    internal static class ResultExtensions
    {
        public static Result<T> AsResult<T>(this T value) => new Result<T>(value);
    }
}
