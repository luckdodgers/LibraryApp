using LibraryApp.Application.Common.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
