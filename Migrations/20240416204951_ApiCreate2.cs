using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sellasist_Optima.Migrations
{
    /// <inheritdoc />
    public partial class ApiCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "sellasistapi",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_sellasistapi_UserId",
                table: "sellasistapi",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_sellasistapi_AspNetUsers_UserId",
                table: "sellasistapi",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sellasistapi_AspNetUsers_UserId",
                table: "sellasistapi");

            migrationBuilder.DropIndex(
                name: "IX_sellasistapi_UserId",
                table: "sellasistapi");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "sellasistapi",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
