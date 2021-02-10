using LibraryApp.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Application.Application.Common.Models
{
    public interface IRequestResult
    {
        public bool Succeeded { get; }
        public RequestError ErrorType { get; }
        public string[] Errors { get; }

        abstract string ErrorsToString();
    }
}
