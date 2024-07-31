using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.BusinessData.Migrations
{
    /// <inheritdoc />
    public partial class update_table_supplier_order_staff_receiving_staff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupplierOrders_Staffs_ReceivingStaffId",
                table: "SupplierOrders");

            migrationBuilder.DropIndex(
                name: "IX_SupplierOrders_ReceivingStaffId",
                table: "SupplierOrders");

            migrationBuilder.DropColumn(
                name: "ReceivingStaffId",
                table: "SupplierOrders");

            migrationBuilder.AddColumn<string>(
                name: "ReceivingStaff",
                table: "SupplierOrders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceivingStaff",
                table: "SupplierOrders");

            migrationBuilder.AddColumn<int>(
                name: "ReceivingStaffId",
                table: "SupplierOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupplierOrders_ReceivingStaffId",
                table: "SupplierOrders",
                column: "ReceivingStaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierOrders_Staffs_ReceivingStaffId",
                table: "SupplierOrders",
                column: "ReceivingStaffId",
                principalTable: "Staffs",
                principalColumn: "Id");
        }
    }
}
