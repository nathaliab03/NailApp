using Microsoft.AspNetCore.Identity;

namespace NailApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}