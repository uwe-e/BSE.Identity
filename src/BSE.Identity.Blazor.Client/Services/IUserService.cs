using BSE.Identity.Blazor.Client.Models;
using BSE.Identity.Blazor.Client.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace BSE.Identity.Blazor.Client.Services
{
    public interface IUserService
    {
        Task<IdentityResult> ChangePasswordAsync(ChangePasswordViewModel model);
        Task<IdentityResult> CreateUserAsync(CreateUserViewModel model);
        Task<bool> DeleteUserAsync(string userId);
        Task<ApplicationUser?> GetUserByIdAsync(string userId);
        Task<IQueryable<UserRolesViewModel>> GetAllUsersWithRolesAsync();
        Task<IdentityResult> UpdateUserRolesAsync(UserRolesViewModel model, IList<RoleViewModel> roles);
    }
}
