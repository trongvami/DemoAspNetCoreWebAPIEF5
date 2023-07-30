using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerSide.Migrations
{
    public partial class addDataRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "35bdeddf-b0eb-4024-9121-f1b8ca7a8787", "2", "User", "User" },
                    { "52b21e4d-9515-4349-993a-d216f2744f77", "5", "Leader", "Leader" },
                    { "ab0cfd4f-cae5-4ac9-9815-1b90376d503d", "3", "Admin", "Admin" },
                    { "cb6c3fe6-dfd3-4dd9-a397-da5905ae310d", "1", "Customer", "Customer" },
                    { "f2efdcc7-1a5d-44a7-9e1e-bb404ba5236a", "4", "Manager", "Manager" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "35bdeddf-b0eb-4024-9121-f1b8ca7a8787");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "52b21e4d-9515-4349-993a-d216f2744f77");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ab0cfd4f-cae5-4ac9-9815-1b90376d503d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cb6c3fe6-dfd3-4dd9-a397-da5905ae310d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f2efdcc7-1a5d-44a7-9e1e-bb404ba5236a");
        }
    }
}
