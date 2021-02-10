using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Infrastructure.Identity.Models.ChangeRole
{
    public class ChangeRoleRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
