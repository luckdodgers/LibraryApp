﻿using LibraryApp.Application.Application.Common.Models;
using LibraryApp.Application.Common.Enums;
using System.Collections.Generic;
using System.Linq;

namespace LibraryApp.Application.Common.Models
{
    public abstract class BaseResult : IRequestResult
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
