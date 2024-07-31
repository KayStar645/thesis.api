using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.BusinessData.Migrations
{
    /// <inheritdoc />
    public partial class update_table_detail_order_isselected : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSelected",
                table: "DetailOrders",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSelected",
                table: "DetailOrders");
        }
    }
}
