using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sellasist_Optima.Migrations.SellAsist
{
    /// <inheritdoc />
    public partial class Apitoken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TextAPI",
                table: "SellAsistAPI",
                newName: "KeyAPI");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "KeyAPI",
                table: "SellAsistAPI",
                newName: "TextAPI");
        }
    }
}
