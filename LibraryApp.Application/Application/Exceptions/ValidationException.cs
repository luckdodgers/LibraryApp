using System;

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
