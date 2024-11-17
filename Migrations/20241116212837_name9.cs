using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sellasist_Optima.Migrations.Configuration
{
    /// <inheritdoc />
    public partial class name9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EAN",
                table: "WebApiProduct",
                newName: "barcode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "barcode",
                table: "WebApiProduct",
                newName: "EAN");
        }
    }
}
