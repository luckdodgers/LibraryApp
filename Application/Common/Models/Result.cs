using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Application.Common.Models
{
    public class Result
    {
        public bool Succeeded { get; private set; }
        public string[] Errors { get; private set; }

        public Result(bool succeeded, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
        }

        public static Result Success() => new Result(true, new string[0]);

        public static Result Fail(IEnumerable<string> errors) => new Result(false, errors);

        public static Result Fail(string error) => new Result(false, new string[] { error });

        public string ErrorsToString()
        {
            if (Errors?.Length > 0)
                return string.Join('\n', Errors);

            return string.Empty;
        }
    }
}
