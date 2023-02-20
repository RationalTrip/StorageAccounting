using System;
using System.Linq.Expressions;

namespace StorageAccounting.Database.Extensions
{
    internal static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> EqualTo<T, TVal>(this Expression<Func<T, TVal>> property,
            TVal value)
        {
            var expressionBody = Expression.Equal(property.Body, Expression.Constant(value));

            return Expression.Lambda<Func<T, bool>>(expressionBody, property.Parameters);
        }
    }
}
