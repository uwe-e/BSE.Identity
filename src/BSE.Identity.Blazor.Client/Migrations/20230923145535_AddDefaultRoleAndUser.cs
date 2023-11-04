using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSE.Identity.Blazor.Client.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultRoleAndUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "332ce434-e26f-4307-97f3-e3d411fecc07", "332ce434-e26f-4307-97f3-e3d411fecc07", "Administator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "3bbd9329-5316-425a-be8c-fa514c50a5b1", 0, "30034850-4ff4-4880-8523-172846cb6920", "admin@bsetunes.com", true, "", "", false, null, null, "ADMIN@BSETUNES.COM", "AQAAAAIAAYagAAAAECrXMhh57xgGxk+Zq/ulmdwQRRi2LHnGyY0kIRfsYGN9QTLiuHhEx9RqBOFZwWQQEw==", null, false, "236e9dfc-8e56-420b-ad7b-9aeeb6e00757", false, "admin@bsetunes.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "332ce434-e26f-4307-97f3-e3d411fecc07", "3bbd9329-5316-425a-be8c-fa514c50a5b1" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "332ce434-e26f-4307-97f3-e3d411fecc07", "3bbd9329-5316-425a-be8c-fa514c50a5b1" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "332ce434-e26f-4307-97f3-e3d411fecc07");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3bbd9329-5316-425a-be8c-fa514c50a5b1");
        }
    }
}
