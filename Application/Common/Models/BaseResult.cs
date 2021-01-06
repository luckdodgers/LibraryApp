using LibraryApp.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Application.Common.Models
{
    public abstract class BaseResult
    {
        public bool Succeeded { get; }
        public RequestError ErrorType { get; } = RequestError.None;
        public string[] Errors { get; protected set; }

        protected BaseResult(bool succeeded, RequestError errorType, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            ErrorType = errorType;
            Errors = errors.ToArray();
        }

        public string ErrorsToString()
        {
            if (Errors?.Length > 0)
                return string.Join('\n', Errors);

            return string.Empty;
        }
    }
}
