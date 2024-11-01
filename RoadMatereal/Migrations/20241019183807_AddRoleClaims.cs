using Microsoft.EntityFrameworkCore.Migrations;

namespace RoadMatereal.Migrations
{
    public partial class AddRoleClaims : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "RoleId", "ClaimType", "ClaimValue" },
                values: new object[,]
                {
                    { 1, 1, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Admin" },
                    { 2, 2, "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "User" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);
        }
    }

}