using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tm20trial.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceValidationByStateMap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Validate",
                table: "Maps");

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Maps",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Maps");

            migrationBuilder.AddColumn<bool>(
                name: "Validate",
                table: "Maps",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
