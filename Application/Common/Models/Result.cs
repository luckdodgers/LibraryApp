using LibraryApp.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Application.Common.Models
{
    public class Result
    {
        public bool Succeeded { get; }
        public RequestError ErrorType { get; } = RequestError.None;
        public string[] Errors { get; private set; }

        private Result(bool succeeded, RequestError errorType, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            ErrorType = errorType;
            Errors = errors.ToArray();
        }

        public static Result Success() => new Result(true, RequestError.None, new string[0]);

        public static Result Fail(RequestError errorType, IEnumerable<string> errors) => new Result(false, errorType, errors);

        public static Result Fail(RequestError errorType, string error) => new Result(false, errorType, new string[] { error });

        public static Result InternalError(string message = "Internal error") => Fail(RequestError.ApplicationException, message);

        public string ErrorsToString()
        {
            if (Errors?.Length > 0)
                return string.Join('\n', Errors);

            return string.Empty;
        }
    }
}
