using Microsoft.EntityFrameworkCore.Migrations;

namespace QuanLyBanHangCore.Migrations
{
    public partial class AddSoLuongSanPham : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SoLuong",
                table: "Products",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SoLuong",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
