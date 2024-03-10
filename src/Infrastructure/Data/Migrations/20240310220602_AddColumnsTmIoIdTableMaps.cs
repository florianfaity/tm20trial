using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tm20trial.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnsTmIoIdTableMaps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TmIoId",
                table: "Maps",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TmIoId",
                table: "Maps");
        }
    }
}
