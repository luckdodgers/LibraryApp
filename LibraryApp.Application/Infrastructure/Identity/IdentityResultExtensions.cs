using LibraryApp.Application.Common.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace LibraryApp.Application.Infrastructure.Identity
{
    public static class IdentityResultExtensions
    {
        public static BaseResult ToApplicationResult(this IdentityResult result)
        {
            return result.Succeeded
                ? RequestResult.Success()
                : RequestResult.Fail(Common.Enums.RequestError.OtherError, result.Errors.Select(e => e.Description));
        }
    }
}
