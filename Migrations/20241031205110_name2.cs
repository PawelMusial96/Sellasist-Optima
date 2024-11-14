using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sellasist_Optima.Migrations.Configuration
{
    /// <inheritdoc />
    public partial class name2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Login",
                table: "WebApiClient",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "KeyWebAPI",
                table: "WebApiClient",
                newName: "TokenAPI");

            migrationBuilder.AddColumn<string>(
                name: "Grant_type",
                table: "WebApiClient",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grant_type",
                table: "WebApiClient");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "WebApiClient",
                newName: "Login");

            migrationBuilder.RenameColumn(
                name: "TokenAPI",
                table: "WebApiClient",
                newName: "KeyWebAPI");
        }
    }
}
