using Microsoft.EntityFrameworkCore.Migrations;

namespace QuanLyBanHangCore.Migrations
{
    public partial class SuaTenBangCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categies_CategoryID",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categies",
                table: "Categies");

            migrationBuilder.RenameTable(
                name: "Categies",
                newName: "Categories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryID",
                table: "Products",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryID",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Categies");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categies",
                table: "Categies",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categies_CategoryID",
                table: "Products",
                column: "CategoryID",
                principalTable: "Categies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
