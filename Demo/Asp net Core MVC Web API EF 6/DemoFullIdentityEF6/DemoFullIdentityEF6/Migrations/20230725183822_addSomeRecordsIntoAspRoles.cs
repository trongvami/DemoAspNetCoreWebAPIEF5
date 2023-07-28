using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoFullIdentityEF6.Migrations
{
    public partial class addSomeRecordsIntoAspRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "90d824b9-8a6e-4277-b599-00f67a8b310b", "4", "Leader", "Leader" },
                    { "bbe0f6d6-9842-462a-bee3-9b325f53ee58", "2", "Admin", "Admin" },
                    { "d87054ee-be1e-4cf0-b30f-f8a31914b9c2", "1", "User", "User" },
                    { "e362cdbe-2d8e-48e8-8f77-be5ba4d2683f", "3", "Manager", "Manager" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "90d824b9-8a6e-4277-b599-00f67a8b310b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bbe0f6d6-9842-462a-bee3-9b325f53ee58");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d87054ee-be1e-4cf0-b30f-f8a31914b9c2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e362cdbe-2d8e-48e8-8f77-be5ba4d2683f");
        }
    }
}
