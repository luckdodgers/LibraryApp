using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Application.Exceptions
{
    [Serializable]
    public class ValidationException : Exception
    {
        public ValidationException() { }

        public ValidationException(string message) { }

        public ValidationException(string message, Exception innerException) { }
    }
}
