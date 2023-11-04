using Microsoft.AspNetCore.Identity;

namespace BSE.Identity.Blazor.Client.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
