using System;
using System.Diagnostics.Contracts;

namespace StorageAccounting.Domain.Common
{
    public readonly struct Result
    {
        private ResultState State { get; }
        public Exception Exception { get; }

        /// <summary>
        /// Constructor of a success result
        /// </summary>
        public Result()
        {
            State = ResultState.Success;
            Exception = default!;
        }

        /// <summary>
        /// Constructor of an error value
        /// </summary>
        /// <param name="e"></param>
        public Result(Exception e)
        {
            State = ResultState.Faulted;
            Exception = e;
        }

        public static Result Success => new Result();

        public Result<T> AsFaultResult<T>() =>
            IsFaulted ?
                new Result<T>(Exception) :
                throw new InvalidOperationException("Operation not accessible for successful results.");

        /// <summary>
        /// True if the result is faulted
        /// </summary>
        [Pure]
        public bool IsFaulted =>
            State == ResultState.Faulted;

        /// <summary>
        /// True if the struct is in an invalid state
        /// </summary>
        [Pure]
        public bool IsBottom =>
            State == ResultState.Faulted && Exception == null;

        /// <summary>
        /// True if the struct is in an success
        /// </summary>
        [Pure]
        public bool IsSuccess =>
            State == ResultState.Success;

        /// <summary>
        /// Convert the value to a showable string
        /// </summary>
        [Pure]
        public override string ToString() =>
            IsFaulted
                ? Exception?.ToString() ?? "(Bottom)"
                : "(Success)";

        [Pure]
        public R Match<R>(Func<R> Succ, Func<Exception, R> Fail) =>
            IsFaulted
                ? Fail(Exception)
                : Succ();

        private enum ResultState : byte
        {
            Faulted,
            Success
        }
    }
}
