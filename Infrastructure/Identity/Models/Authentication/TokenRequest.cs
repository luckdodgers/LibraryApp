using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Infrastructure.Identity.Models.Authentication
{
    public class TokenRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
