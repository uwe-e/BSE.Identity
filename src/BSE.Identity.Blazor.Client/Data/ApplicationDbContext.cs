using BSE.Identity.Blazor.Client.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BSE.Identity.Blazor.Client.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            /*
             * Seed Identity User and Role In ASP.Net Core with EF Core
             * https://frankofoedu.medium.com/how-to-seed-identity-role-with-associated-user-in-asp-net-core-ef-core-e40ecd643d0f
             */

            //builder.ApplyConfiguration(new RoleConfiguration());
            string ADMIN_ID = "3bbd9329-5316-425a-be8c-fa514c50a5b1";
            string ROLE_ID = "332ce434-e26f-4307-97f3-e3d411fecc07";

            //seed admin role
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR",
                Id = ROLE_ID,
                ConcurrencyStamp = ROLE_ID
            });

            //create user
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

            //set user password
            PasswordHasher<ApplicationUser> ph = new PasswordHasher<ApplicationUser>();
            appUser.PasswordHash = ph.HashPassword(appUser, "123456_?");

            //seed user
            builder.Entity<ApplicationUser>().HasData(appUser);

            //set user role to admin
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ROLE_ID,
                UserId = ADMIN_ID
            });

            base.OnModelCreating(builder);
        }
    }
}