using BSE.Identity.Blazor.Client.Data;
using BSE.Identity.Blazor.Client.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BSE.Identity.Blazor.Client.Services
{
    public class RoleService : IRoleService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(IDbContextFactory<ApplicationDbContext> contextFactory, RoleManager<IdentityRole> RoleManager)
        {
            _contextFactory = contextFactory;
            _roleManager = RoleManager;
        }

        public async Task<IdentityResult> CreateRoleAsync(string roleName)
        {
            IdentityRole identityRole = new IdentityRole
            {
                Name = roleName
            };
            return await _roleManager.CreateAsync(identityRole);
        }

        public async Task<IdentityResult> DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId).ConfigureAwait(false);
            if (role == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Role with ID '{roleId}' not found." });
            }
            return await _roleManager.DeleteAsync(role).ConfigureAwait(false);
        }

        public async Task<IdentityRole?> GetRoleByIdAsync(string roleId)
        {
            return await _roleManager.FindByIdAsync(roleId).ConfigureAwait(false);
        }

        public Task<IQueryable<RoleViewModel>> GetRolesAsync()
        {
            return Task.Run(GetRoles);
        }

        public async Task<IList<RoleViewModel>> GetRolesByUserAsync(string userId)
        {
            using var context = _contextFactory.CreateDbContext();

            // Fix: Call the method inside Task.Run using a lambda to defer execution
            return await Task.Run(() => GetRolesByUser(context, userId));
        }

        public async Task<IdentityResult> UpdateRoleAsync(RoleViewModel model)
        {
            var identityRole = await _roleManager.FindByIdAsync(model.Id).ConfigureAwait(false);
            if (identityRole is null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Role with ID '{model.Id}' not found." });
            }

            // Only update if the name has actually changed to avoid unnecessary DB writes
            if (!string.Equals(identityRole.Name, model.Name, StringComparison.Ordinal))
            {
                identityRole.Name = model.Name;
                return await _roleManager.UpdateAsync(identityRole).ConfigureAwait(false);
            }

            // If no change, return success without updating
            return IdentityResult.Success;
        }

        private IQueryable<RoleViewModel> GetRoles()
        {
            // because availability of the roles, the IQueryable has to be converted from a list
            var roles = _roleManager.Roles.Select(role => new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name ?? string.Empty
            }).OrderBy(r => r.Name)
            .ToList();
            
            return roles.AsQueryable();
        }

        private IList<RoleViewModel> GetRolesByUser(ApplicationDbContext context, string userId)
        {
            ArgumentNullException.ThrowIfNull(context);
            
            return [.. context.Roles.GroupJoin(
                context.UserRoles,
                role => role.Id,
                userrole => userrole.RoleId,
                (role, userroles) => new RoleViewModel
                    {
                        Id = role.Id,
                        Name = role.Name ?? string.Empty,
                        IsSelected = userroles.Where(ur => ur.UserId.Equals(userId)).Any()
                    }
                )
                .OrderBy(r => r.Name)];
        }
    }
}
