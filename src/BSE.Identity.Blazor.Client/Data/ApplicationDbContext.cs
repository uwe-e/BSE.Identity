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

        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            string ADMIN_ID = "3bbd9329-5316-425a-be8c-fa514c50a5b1";
            string ROLE_ID = "332ce434-e26f-4307-97f3-e3d411fecc07";

            // Seed Administrator role
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR",
                Id = ROLE_ID,
                ConcurrencyStamp = ROLE_ID
            });

            // Seed default admin user
            // Note: ConcurrencyStamp and SecurityStamp must be set to static values to prevent
            // EF Core migration errors. These properties default to Guid.NewGuid() which causes
            // the model to change on each build.

            // administrator account for loggin in to a development environment.
            // Please do not use this account in the production environment.
            // Email address: admin@bsetunes.com
            // Password: 123456_?

            var appUser = new ApplicationUser
            {
                Id = ADMIN_ID,
                Email = "admin@bsetunes.com",
                EmailConfirmed = true,
                UserName = "admin@bsetunes.com",
                NormalizedUserName = "ADMIN@BSETUNES.COM",
                NormalizedEmail = "ADMIN@BSETUNES.COM",
                FirstName = "",
                LastName = "",
                PasswordHash = "AQAAAAIAAYagAAAAEKtU537XCAkzglrSdCxX24bp0MUT4ZSMDBRCu15Ygki4OzvAVrqpSj4pKYYKuioBew==",
                ConcurrencyStamp = ADMIN_ID,
                SecurityStamp = ADMIN_ID
            };

            builder.Entity<ApplicationUser>().HasData(appUser);

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ROLE_ID,
                UserId = ADMIN_ID
            });

        }
    }
}