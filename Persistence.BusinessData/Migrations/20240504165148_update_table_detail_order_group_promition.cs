using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.BusinessData.Migrations
{
    /// <inheritdoc />
    public partial class update_table_detail_order_group_promition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupPromotion",
                table: "DetailOrders",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupPromotion",
                table: "DetailOrders");
        }
    }
}
