using LibraryApp.Application.Common.Enums;
using System.Collections.Generic;

namespace LibraryApp.Application.Common.Models
{
    public class CommandResult : BaseResult
    {
        private CommandResult(bool succeeded, RequestError errorType, IEnumerable<string> errors) : base(succeeded, errorType, errors)
        {
        }

        public static CommandResult Success() => new CommandResult(true, RequestError.None, new string[0]);

        public static CommandResult Fail(RequestError errorType, IEnumerable<string> errors) => new CommandResult(false, errorType, errors);

        public static CommandResult Fail(RequestError errorType, string error) => new CommandResult(false, errorType, new string[] { error });

        public static CommandResult InternalError(string message = "Internal error") => Fail(RequestError.ApplicationException, message);
    }
}
