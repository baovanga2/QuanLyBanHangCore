using Microsoft.EntityFrameworkCore.Migrations;

namespace QuanLyBanHangCore.Migrations
{
    public partial class SuaKhoaChinhDetailOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DetailOrders",
                table: "DetailOrders");

            migrationBuilder.DropIndex(
                name: "IX_DetailOrders_OrderID",
                table: "DetailOrders");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "DetailOrders");

            migrationBuilder.AddColumn<decimal>(
                name: "Gia",
                table: "DetailOrders",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetailOrders",
                table: "DetailOrders",
                columns: new[] { "OrderID", "ProductID" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DetailOrders",
                table: "DetailOrders");

            migrationBuilder.DropColumn(
                name: "Gia",
                table: "DetailOrders");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "DetailOrders",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetailOrders",
                table: "DetailOrders",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_DetailOrders_OrderID",
                table: "DetailOrders",
                column: "OrderID");
        }
    }
}
