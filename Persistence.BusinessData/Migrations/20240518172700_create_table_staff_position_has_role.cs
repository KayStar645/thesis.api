using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.BusinessData.Migrations
{
    /// <inheritdoc />
    public partial class create_table_staff_position_has_role : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StaffPositionHasRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    StaffPositionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffPositionHasRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffPositionHasRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StaffPositionHasRoles_StaffPositions_StaffPositionId",
                        column: x => x.StaffPositionId,
                        principalTable: "StaffPositions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StaffPositionHasRoles_RoleId",
                table: "StaffPositionHasRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffPositionHasRoles_StaffPositionId",
                table: "StaffPositionHasRoles",
                column: "StaffPositionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StaffPositionHasRoles");
        }
    }
}
