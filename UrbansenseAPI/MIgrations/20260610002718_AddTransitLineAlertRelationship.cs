using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrbansenseAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddTransitLineAlertRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ID_TRANSIT_LINE",
                table: "ALERTS",
                type: "NUMBER(19)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ALERTS_ID_TRANSIT_LINE",
                table: "ALERTS",
                column: "ID_TRANSIT_LINE");

            migrationBuilder.AddForeignKey(
                name: "FK_ALERTS_TRANSIT_LINES_ID_TRANSIT_LINE",
                table: "ALERTS",
                column: "ID_TRANSIT_LINE",
                principalTable: "TRANSIT_LINES",
                principalColumn: "ID_LINE",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ALERTS_TRANSIT_LINES_ID_TRANSIT_LINE",
                table: "ALERTS");

            migrationBuilder.DropIndex(
                name: "IX_ALERTS_ID_TRANSIT_LINE",
                table: "ALERTS");

            migrationBuilder.DropColumn(
                name: "ID_TRANSIT_LINE",
                table: "ALERTS");
        }
    }
}
