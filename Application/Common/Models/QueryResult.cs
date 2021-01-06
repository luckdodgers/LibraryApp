using LibraryApp.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Application.Common.Models
{
    public class QueryResult<T> : BaseResult
    {
        /// <summary>
        /// Query result data
        /// </summary>
        public T Value { get; }

        public QueryResult(bool succeeded, RequestError errorType, IEnumerable<string> errors, T value = default) : base(succeeded, errorType, errors)
        {
            Value = value;
        }

        public static QueryResult<T> Success(T resultValue) => new QueryResult<T>(true, RequestError.None, new string[0], resultValue);

        public static QueryResult<T> Fail(RequestError errorType, IEnumerable<string> errors) => new QueryResult<T>(false, errorType, errors);

        public static QueryResult<T> Fail(RequestError errorType, string error) => new QueryResult<T>(false, errorType, new string[] { error });

        public static QueryResult<T> InternalError(string message = "Internal error") => Fail(RequestError.ApplicationException, message);
    }
}
