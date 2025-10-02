using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class ManyToMany_BranchItemSalesInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BranchItemSalesInvoices",
                columns: table => new
                {
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    OperationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchItemSalesInvoices", x => new { x.BranchId, x.ItemId, x.OperationId });
                    table.ForeignKey(
                        name: "FK_BranchItemSalesInvoices_BranchItems_BranchId_ItemId",
                        columns: x => new { x.BranchId, x.ItemId },
                        principalTable: "BranchItems",
                        principalColumns: new[] { "BranchId", "ItemId" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BranchItemSalesInvoices_SalesInvoices_OperationId",
                        column: x => x.OperationId,
                        principalTable: "SalesInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BranchItemSalesInvoices_OperationId",
                table: "BranchItemSalesInvoices",
                column: "OperationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BranchItemSalesInvoices");
        }
    }
}
