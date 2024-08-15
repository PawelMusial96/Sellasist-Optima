using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sellasist_Optima.Migrations.SellAsist
{
    /// <inheritdoc />
    public partial class tokenapi3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameShop",
                table: "SellAsistAPI");

            migrationBuilder.AddColumn<string>(
                name: "API",
                table: "SellAsistAPI",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "API",
                table: "SellAsistAPI");

            migrationBuilder.AddColumn<string>(
                name: "NameShop",
                table: "SellAsistAPI",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
