using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lib.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "Games",
                newName: "StarTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StarTime",
                table: "Games",
                newName: "StartTime");
        }
    }
}
