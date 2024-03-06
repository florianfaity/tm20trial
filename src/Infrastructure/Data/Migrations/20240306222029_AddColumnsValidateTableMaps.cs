using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tm20trial.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnsValidateTableMaps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Validate",
                table: "Maps",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Validate",
                table: "Maps");
        }
    }
}
