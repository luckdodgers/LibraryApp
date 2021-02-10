using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Infrastructure.Identity.Models.AddRole
{
    public class AddRoleRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
