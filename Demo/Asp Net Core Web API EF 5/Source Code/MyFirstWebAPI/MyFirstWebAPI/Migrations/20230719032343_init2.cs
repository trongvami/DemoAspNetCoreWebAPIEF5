using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFirstWebAPI.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaLoai",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    MaLoai = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLoai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.MaLoai);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_MaLoai",
                table: "Product",
                column: "MaLoai");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Category_MaLoai",
                table: "Product",
                column: "MaLoai",
                principalTable: "Category",
                principalColumn: "MaLoai",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Category_MaLoai",
                table: "Product");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Product_MaLoai",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "MaLoai",
                table: "Product");
        }
    }
}
