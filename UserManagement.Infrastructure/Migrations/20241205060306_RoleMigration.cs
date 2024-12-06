using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RoleMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b0a0000e-2770-4002-919d-1b04318f4102", null, "Super", "SUPER" },
                    { "df643eba-13fc-4145-a77d-e5831ce17dd6", null, "Admin", "ADMIN" },
                    { "feca1bcf-23c4-491e-8160-504c56a50c84", null, "Regular", "REGULAR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b0a0000e-2770-4002-919d-1b04318f4102");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "df643eba-13fc-4145-a77d-e5831ce17dd6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "feca1bcf-23c4-491e-8160-504c56a50c84");
        }
    }
}
