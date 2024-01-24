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
            migrationBuilder.AlterColumn<string>(
                name: "GameStatus",
                table: "Games",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "BeingPrepared",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 5);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "GameStatus",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 5,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "BeingPrepared");
        }
    }
}
