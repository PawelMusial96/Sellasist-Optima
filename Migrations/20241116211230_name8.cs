using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sellasist_Optima.Migrations.Configuration
{
    /// <inheritdoc />
    public partial class name8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SellAsistProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EAN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellAsistProduct", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebApiProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EAN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebApiProduct", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductMappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellAsistProductId = table.Column<int>(type: "int", nullable: false),
                    WebApiProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductMappings_SellAsistProduct_SellAsistProductId",
                        column: x => x.SellAsistProductId,
                        principalTable: "SellAsistProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductMappings_WebApiProduct_WebApiProductId",
                        column: x => x.WebApiProductId,
                        principalTable: "WebApiProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductMappings_SellAsistProductId",
                table: "ProductMappings",
                column: "SellAsistProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMappings_WebApiProductId",
                table: "ProductMappings",
                column: "WebApiProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductMappings");

            migrationBuilder.DropTable(
                name: "SellAsistProduct");

            migrationBuilder.DropTable(
                name: "WebApiProduct");
        }
    }
}
