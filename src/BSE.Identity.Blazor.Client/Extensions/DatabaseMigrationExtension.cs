using BSE.Identity.Blazor.Client.Data;
using BSE.Identity.Blazor.Client.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BSE.Identity.Blazor.Client.Extensions
{
    public static class DatabaseMigrationExtension
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            
            using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            string ADMIN_ID = "3bbd9329-5316-425a-be8c-fa514c50a5b1";
            string ROLE_ID = "332ce434-e26f-4307-97f3-e3d411fecc07";

            if (!context.Roles.Any())
            {
                context.Roles.Add(new IdentityRole
                {
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR",
                    Id = ROLE_ID,
                    ConcurrencyStamp = ROLE_ID
                });

                context.SaveChanges();
            }

            if (!context.Users.Any())
            {

                var appUser = new ApplicationUser
                {
                    Id = ADMIN_ID,
                    Email = "admin@bsetunes.com",
                    EmailConfirmed = true,
                    UserName = "admin@bsetunes.com",
                    FirstName = "",
                    LastName = "",
                    NormalizedUserName = "ADMIN@BSETUNES.COM"
                };

                //hash user password
                PasswordHasher<ApplicationUser> ph = new PasswordHasher<ApplicationUser>();
                appUser.PasswordHash = ph.HashPassword(appUser, "123456_?");

                context.Users.Add(appUser);

                context.SaveChanges();
            }

            if (!context.UserRoles.Any())
            {
                context.UserRoles.Add(new IdentityUserRole<string>
                {
                    RoleId = ROLE_ID,
                    UserId = ADMIN_ID
                });

                context.SaveChanges();
            }

            return host;
        }
    }
}
