using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitMigrationNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2cef8fa2-efb0-4dc9-9f37-144d9a6adb92");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "60cb3a4c-40d3-4c9c-9180-97bdfa50e0b1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "87f8e633-efda-46ae-9b2c-c196341d498b");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2cef8fa2-efb0-4dc9-9f37-144d9a6adb92", null, "Super", "SUPER" },
                    { "60cb3a4c-40d3-4c9c-9180-97bdfa50e0b1", null, "Admin", "ADMIN" },
                    { "87f8e633-efda-46ae-9b2c-c196341d498b", null, "Regular", "REGULAR" }
                });
        }
    }
}
