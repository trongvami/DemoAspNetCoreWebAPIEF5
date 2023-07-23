using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroceryStoreEF6.Migrations
{
    public partial class initSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1692a90d-bd9e-49c7-a953-42b30138e3fa", "2", "User", "User" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "54584c5e-a286-4740-9a2f-d721ae8d50ae", "3", "Manager", "Manager" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ea62cc9b-a3f8-404d-a277-bfd498da1874", "1", "Admin", "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1692a90d-bd9e-49c7-a953-42b30138e3fa");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "54584c5e-a286-4740-9a2f-d721ae8d50ae");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ea62cc9b-a3f8-404d-a277-bfd498da1874");
        }
    }
}
