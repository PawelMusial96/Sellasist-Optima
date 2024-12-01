using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sellasist_Optima.Migrations
{
    /// <inheritdoc />
    public partial class name1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttributeMappings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WebApiAttributeId = table.Column<int>(type: "int", nullable: false),
                    SellAsistAttributeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeMappings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatabaseName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SellAsistAPI",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShopName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    KeyAPI = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellAsistAPI", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SellAsistProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EAN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellAsistProduct", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebApiClient",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Grant_type = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Localhost = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TokenAPI = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebApiClient", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebApiProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Inactive = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManufacturerCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VatRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VatRateFlag = table.Column<int>(type: "int", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupplierCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CatalogNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PackageDeposit = table.Column<int>(type: "int", nullable: true),
                    Product = table.Column<int>(type: "int", nullable: false),
                    DefaultGroup = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SalesCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: true),
                    Width = table.Column<int>(type: "int", nullable: true),
                    Length = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Updated = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyDataId = table.Column<int>(type: "int", nullable: false),
                    GettingElementsForFSPA = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebApiProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebApiProduct_CompanyData_CompanyDataId",
                        column: x => x.CompanyDataId,
                        principalTable: "CompanyData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attribute",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebApiProductId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attribute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attribute_WebApiProduct_WebApiProductId",
                        column: x => x.WebApiProductId,
                        principalTable: "WebApiProduct",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Price",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OptimaItemId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemBarcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VatRate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Error = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebApiProductId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Price", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Price_WebApiProduct_WebApiProductId",
                        column: x => x.WebApiProductId,
                        principalTable: "WebApiProduct",
                        principalColumn: "Id");
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
                name: "IX_Attribute_WebApiProductId",
                table: "Attribute",
                column: "WebApiProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Price_WebApiProductId",
                table: "Price",
                column: "WebApiProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMappings_SellAsistProductId",
                table: "ProductMappings",
                column: "SellAsistProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMappings_WebApiProductId",
                table: "ProductMappings",
                column: "WebApiProductId");

            migrationBuilder.CreateIndex(
                name: "IX_WebApiProduct_CompanyDataId",
                table: "WebApiProduct",
                column: "CompanyDataId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attribute");

            migrationBuilder.DropTable(
                name: "AttributeMappings");

            migrationBuilder.DropTable(
                name: "Price");

            migrationBuilder.DropTable(
                name: "ProductMappings");

            migrationBuilder.DropTable(
                name: "SellAsistAPI");

            migrationBuilder.DropTable(
                name: "WebApiClient");

            migrationBuilder.DropTable(
                name: "SellAsistProduct");

            migrationBuilder.DropTable(
                name: "WebApiProduct");

            migrationBuilder.DropTable(
                name: "CompanyData");
        }
    }
}
