using BSE.Identity.Blazor.Client.ViewModels;

namespace BSE.Identity.Blazor.Client.Models
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }

        public IEnumerable<RoleViewModel> Roles123 { get; set; }
    }
}
