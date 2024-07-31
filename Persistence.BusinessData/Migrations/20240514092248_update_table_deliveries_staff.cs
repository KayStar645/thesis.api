using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.BusinessData.Migrations
{
    /// <inheritdoc />
    public partial class update_table_deliveries_staff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PackingStaffId",
                table: "Deliveries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShipperId",
                table: "Deliveries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_PackingStaffId",
                table: "Deliveries",
                column: "PackingStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_ShipperId",
                table: "Deliveries",
                column: "ShipperId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Staffs_PackingStaffId",
                table: "Deliveries",
                column: "PackingStaffId",
                principalTable: "Staffs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Staffs_ShipperId",
                table: "Deliveries",
                column: "ShipperId",
                principalTable: "Staffs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Staffs_PackingStaffId",
                table: "Deliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Staffs_ShipperId",
                table: "Deliveries");

            migrationBuilder.DropIndex(
                name: "IX_Deliveries_PackingStaffId",
                table: "Deliveries");

            migrationBuilder.DropIndex(
                name: "IX_Deliveries_ShipperId",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "PackingStaffId",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "ShipperId",
                table: "Deliveries");
        }
    }
}
