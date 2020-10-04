using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Application.Common.Enums
{
    public enum RequestError
    {
        None,
        NotFound,
        AlreadyExists,
        ValidationError,
        ApplicationException,
    }
}
