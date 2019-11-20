using Microsoft.EntityFrameworkCore.Migrations;

namespace QuanLyBanHangCore.Migrations
{
    public partial class DoiUshortSangUint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "SoLuong",
                table: "Products",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "SoLuong",
                table: "DetailOrders",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SoLuong",
                table: "Products",
                type: "int",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<int>(
                name: "SoLuong",
                table: "DetailOrders",
                type: "int",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
