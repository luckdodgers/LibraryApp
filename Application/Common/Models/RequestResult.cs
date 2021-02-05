using LibraryApp.Application.Common.Enums;
using System.Collections.Generic;

namespace LibraryApp.Application.Common.Models
{
    public class RequestResult : BaseResult
    {
        private RequestResult(bool succeeded, RequestError errorType, IEnumerable<string> errors) : base(succeeded, errorType, errors)
        {
        }

        public static RequestResult Success() => new RequestResult(true, RequestError.None, new string[0]);

        public static RequestResult Fail(RequestError errorType, IEnumerable<string> errors) => new RequestResult(false, errorType, errors);

        public static RequestResult Fail(RequestError errorType, string error) => new RequestResult(false, errorType, new string[] { error });

        public static RequestResult InternalError(string message = "Internal error") => Fail(RequestError.ApplicationException, message);
    }
}
