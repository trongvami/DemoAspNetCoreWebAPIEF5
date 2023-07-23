using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroceryStoreEF6.Migrations
{
    public partial class asdasd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a05fd401-242d-4c41-907f-e144c56b00bd");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3bf178e4-48cc-4f74-b664-b70fc652de01", "4", "Leader", "Leader" },
                    { "87b57d06-bb51-40f3-b5d4-70d06ec10df2", "2", "User", "User" },
                    { "a14d6a5f-d0ce-4bbe-970b-bb78e8b78265", "3", "Manager", "Manager" },
                    { "ede57b67-b6e5-4541-8093-4b228a790638", "1", "Admin", "Admin" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3bf178e4-48cc-4f74-b664-b70fc652de01");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "87b57d06-bb51-40f3-b5d4-70d06ec10df2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a14d6a5f-d0ce-4bbe-970b-bb78e8b78265");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ede57b67-b6e5-4541-8093-4b228a790638");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a05fd401-242d-4c41-907f-e144c56b00bd", "4", "Leader", "Leader" });
        }
    }
}
