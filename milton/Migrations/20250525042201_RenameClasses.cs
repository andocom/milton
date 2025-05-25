using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace milton.Migrations
{
    /// <inheritdoc />
    public partial class RenameClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PriceSnapshots");

            migrationBuilder.DropTable(
                name: "ScrapeSources");

            migrationBuilder.DropTable(
                name: "PriceSources");

            migrationBuilder.DropIndex(
                name: "IX_Snapshots_SKU_SnapshotDate_Source",
                table: "Snapshots");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Snapshots");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Snapshots");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Snapshots");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Snapshots");

            migrationBuilder.DropColumn(
                name: "SKU",
                table: "Snapshots");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Snapshots");

            migrationBuilder.RenameColumn(
                name: "SnapshotDate",
                table: "Snapshots",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "Sku",
                table: "Products",
                newName: "SKU");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Competitors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaseUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competitors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompetitorProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompetitorId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CompetitorSKU = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitorProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompetitorProducts_Competitors_CompetitorId",
                        column: x => x.CompetitorId,
                        principalTable: "Competitors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompetitorProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductSnapshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CompetitorId = table.Column<int>(type: "int", nullable: false),
                    SnapshotId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductSnapshotId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSnapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSnapshots_Competitors_CompetitorId",
                        column: x => x.CompetitorId,
                        principalTable: "Competitors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductSnapshots_ProductSnapshots_ProductSnapshotId",
                        column: x => x.ProductSnapshotId,
                        principalTable: "ProductSnapshots",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductSnapshots_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductSnapshots_Snapshots_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "Snapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitorProducts_CompetitorId",
                table: "CompetitorProducts",
                column: "CompetitorId");

            migrationBuilder.CreateIndex(
                name: "IX_CompetitorProducts_ProductId",
                table: "CompetitorProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSnapshots_CompetitorId",
                table: "ProductSnapshots",
                column: "CompetitorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSnapshots_ProductId_CompetitorId_SnapshotId",
                table: "ProductSnapshots",
                columns: new[] { "ProductId", "CompetitorId", "SnapshotId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductSnapshots_ProductSnapshotId",
                table: "ProductSnapshots",
                column: "ProductSnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSnapshots_SnapshotId",
                table: "ProductSnapshots",
                column: "SnapshotId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompetitorProducts");

            migrationBuilder.DropTable(
                name: "ProductSnapshots");

            migrationBuilder.DropTable(
                name: "Competitors");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Snapshots",
                newName: "SnapshotDate");

            migrationBuilder.RenameColumn(
                name: "SKU",
                table: "Products",
                newName: "Sku");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Snapshots",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Snapshots",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Snapshots",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ProductId",
                table: "Snapshots",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SKU",
                table: "Snapshots",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Snapshots",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PriceSources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceSources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScrapeSources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScrapeSources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PriceSnapshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PriceSourceId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceSnapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceSnapshots_PriceSources_PriceSourceId",
                        column: x => x.PriceSourceId,
                        principalTable: "PriceSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PriceSnapshots_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Snapshots_SKU_SnapshotDate_Source",
                table: "Snapshots",
                columns: new[] { "SKU", "SnapshotDate", "Source" });

            migrationBuilder.CreateIndex(
                name: "IX_PriceSnapshots_PriceSourceId",
                table: "PriceSnapshots",
                column: "PriceSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceSnapshots_ProductId",
                table: "PriceSnapshots",
                column: "ProductId");
        }
    }
}
