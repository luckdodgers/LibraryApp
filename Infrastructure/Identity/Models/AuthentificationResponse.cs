using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Infrastructure.Identity.Models
{
    public class AuthentificationResponse
    {
        public string Message { get; }
        public bool IsAuthorized { get; }
        public string Token { get; }
        public List<string> Roles { get; }

        public AuthentificationResponse(string message, bool isAuthorized, string token, List<string> roles)
        {
            Message = message;
            IsAuthorized = isAuthorized;
            Token = token;
            Roles = roles;
        }

        public static AuthentificationResponse Denied(string message)
        {
            return new AuthentificationResponse(
                    message: message,
                    isAuthorized: false,
                    token: string.Empty,
                    roles: new List<string>(0)
                );
        }
    }
}
