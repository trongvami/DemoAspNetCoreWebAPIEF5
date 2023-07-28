using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoFullIdentityEF6.Migrations
{
    public partial class addColumnService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "City",
                table: "AspNetUsers",
                newName: "Service");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Service",
                table: "AspNetUsers",
                newName: "City");
        }
    }
}
