using Microsoft.AspNetCore.Identity;

namespace Grad_Project.Entity
{
    public class AppUser:IdentityUser
    {
        public string idNumber { get; set; }
        public DateTime lastLogin { get; set; }
    }
}
