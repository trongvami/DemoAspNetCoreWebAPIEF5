using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GroceryStoreEF6.Migrations
{
    public partial class asdsad : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                values: new object[,]
                {
                    { "44b2bbbe-6d95-4df1-b8ce-2dfb76385142", "2", "Admin", "Admin" },
                    { "7c3a3713-0426-43a1-9aa3-d831b3c84ab6", "1", "User", "User" },
                    { "b092e7fd-6752-4c47-a552-2647746b9a4b", "4", "Leader", "Leader" },
                    { "bb9896c1-85a1-4bbe-ae1f-d5626853694c", "3", "Manager", "Manager" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "44b2bbbe-6d95-4df1-b8ce-2dfb76385142");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7c3a3713-0426-43a1-9aa3-d831b3c84ab6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b092e7fd-6752-4c47-a552-2647746b9a4b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bb9896c1-85a1-4bbe-ae1f-d5626853694c");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "44b2bbbe-6d95-4df1-b8ce-2dfb76385142", "2", "Admin", "Admin" },
                    { "7c3a3713-0426-43a1-9aa3-d831b3c84ab6", "1", "User", "User" },
                    { "b092e7fd-6752-4c47-a552-2647746b9a4b", "4", "Leader", "Leader" },
                    { "bb9896c1-85a1-4bbe-ae1f-d5626853694c", "3", "Manager", "Manager" }
                });
        }
    }
}
