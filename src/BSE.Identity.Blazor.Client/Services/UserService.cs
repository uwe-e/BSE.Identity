using BSE.Identity.Blazor.Client.Data;
using BSE.Identity.Blazor.Client.Models;
using BSE.Identity.Blazor.Client.Shared;
using BSE.Identity.Blazor.Client.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BSE.Identity.Blazor.Client.Services
{
    public class UserService : IUserService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(IDbContextFactory<ApplicationDbContext> contextFactory, UserManager<ApplicationUser> UserManager) {
            _contextFactory = contextFactory;
            _userManager = UserManager;
        }

        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordViewModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            
            var user = await GetUserByIdAsync(model.UserId);
            if (user is null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"User with ID '{model.UserId}' not found." });
            }
            
            return await _userManager.RemovePasswordAsync(user)
                .ContinueWith<IdentityResult>(resultTask =>
                {
                    return _userManager.AddPasswordAsync(user, model.Password).Result;
                });
        }

        public async Task<IdentityResult> CreateUserAsync(CreateUserViewModel model)
        {
            // Avoid null-coalescing for FirstName/LastName if model guarantees non-null (validation layer)
            var newUser = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName ?? string.Empty,
                LastName = model.LastName ?? string.Empty
            };

            // Directly return the Task to avoid unnecessary state machine allocation
            return await _userManager.CreateAsync(newUser, model.Password).ConfigureAwait(false);
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            using var context = _contextFactory.CreateDbContext();

            var userToDelete = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId)
                .ConfigureAwait(false);

            if (userToDelete is null)
                return false;

            // Attach the user entity to the context for deletion if needed
            context.Users.Attach(userToDelete);

            var deletionResult = await _userManager.DeleteAsync(userToDelete).ConfigureAwait(false);
            return deletionResult.Succeeded;
        }

        public async Task<IQueryable<UserRolesViewModel>> GetAllUsersWithRolesAsync()
        {
            using var context = _contextFactory.CreateDbContext();

            // Fix: Call the method inside Task.Run using a lambda to defer execution
            return await Task.Run(() => GetAllUsersWithRoles(context));
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
        }

        public async Task<IdentityResult> UpdateUserRolesAsync(UserRolesViewModel model, IList<RoleViewModel> roles)
        {
            var user = await GetUserByIdAsync(model.UserId);
            if (user is null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"User with ID '{model.UserId}' not found." });
            }

            // Only update fields if they differ to avoid unnecessary DB writes
            bool needsUpdate = false;
            if (user.Email != model.Email)
            {
                user.Email = model.Email;
                needsUpdate = true;
            }
            if (user.FirstName != model.FirstName)
            {
                user.FirstName = model.FirstName ?? string.Empty;
                needsUpdate = true;
            }
            if (user.LastName != model.LastName)
            {
                user.LastName = model.LastName ?? string.Empty;
                needsUpdate = true;
            }
            if (needsUpdate)
            {
                var updateResult = await _userManager.UpdateAsync(user).ConfigureAwait(false);
                if (!updateResult.Succeeded)
                {
                    return updateResult;
                }
            }

            if (roles != null)
            {
                // Avoid unnecessary role removal/addition if roles are unchanged
                var currentRoles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                var newRoles = roles.Where(x => x.IsSelected).Select(y => y.Name).ToList();

                if (!currentRoles.SequenceEqual(newRoles))
                {
                    if (currentRoles.Count > 0)
                    {
                        var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles).ConfigureAwait(false);
                        if (!removeResult.Succeeded)
                        {
                            return IdentityResult.Failed(new IdentityError { Description = $"Failed to remove roles from user '{model.UserName}'." });
                        }
                    }
                    if (newRoles.Count > 0)
                    {
                        var addResult = await _userManager.AddToRolesAsync(user, newRoles).ConfigureAwait(false);
                        if (!addResult.Succeeded)
                        {
                            return IdentityResult.Failed(new IdentityError { Description = $"Failed to add roles to user '{model.UserName}'." });
                        }
                    }
                }
            }
            return IdentityResult.Success;
        }

        private IQueryable<UserRolesViewModel> GetAllUsersWithRoles(ApplicationDbContext context)
        {
            /*
            * this is because of that MySQL behaviour
            * https://mysqlconnector.net/troubleshooting/connection-reuse/
            *
            * the original query should be the use of UserManager.GetRolesAsync(user)
            */

            var usersWithRoles = context.Users.GroupJoin(
                context.UserRoles,
                user => user.Id,
                userrole => userrole.UserId,
                (user, userroles) => new UserRolesViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Roles123 = userroles.Select(ur => new RoleViewModel
                    {
                        Id = ur.RoleId,
                        IsSelected = true,
                        Name = context.Roles
                    .Where(r => r.Id.Equals(ur.RoleId))
                    .Select(r => r.Name).First()
                    }).OrderBy(r => r.Name)
                }).OrderBy(u => u.Email).ToList<UserRolesViewModel>();

            return usersWithRoles.AsQueryable();

        }
    }
}
