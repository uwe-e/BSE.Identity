using BSE.Identity.Blazor.Client.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace BSE.Identity.Blazor.Client.Services
{
    public interface IRoleService
    {
        Task<IdentityResult> CreateRoleAsync(string roleName);
        Task<IdentityResult> DeleteRoleAsync(string roleId);
        Task<IdentityRole?> GetRoleByIdAsync(string roleId);
        Task<IQueryable<RoleViewModel>> GetRolesAsync();
        Task<IList<RoleViewModel>> GetRolesByUserAsync(string userId);
        Task<IdentityResult> UpdateRoleAsync(RoleViewModel model);
    }
}
