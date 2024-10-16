using Microsoft.AspNetCore.Identity;
namespace Vehicle.Domain.Entity.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
    }
}
