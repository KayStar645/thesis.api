using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.BusinessData.Migrations
{
    /// <inheritdoc />
    public partial class update_table_supplier_order_staff_approve : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApproveStaffId",
                table: "SupplierOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupplierOrders_ApproveStaffId",
                table: "SupplierOrders",
                column: "ApproveStaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierOrders_Staffs_ApproveStaffId",
                table: "SupplierOrders",
                column: "ApproveStaffId",
                principalTable: "Staffs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupplierOrders_Staffs_ApproveStaffId",
                table: "SupplierOrders");

            migrationBuilder.DropIndex(
                name: "IX_SupplierOrders_ApproveStaffId",
                table: "SupplierOrders");

            migrationBuilder.DropColumn(
                name: "ApproveStaffId",
                table: "SupplierOrders");
        }
    }
}
