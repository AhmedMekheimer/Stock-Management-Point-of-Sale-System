using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class AlterTables_Discount_Tax_BranchItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscountOperations");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Taxes_RateOrRawValue",
                table: "Taxes");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Discounts_RateOrRawValue",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "RawValue",
                table: "Taxes");

            migrationBuilder.DropColumn(
                name: "DiscountPercentage",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "RestockThreshold",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "TaxPercentage",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "RawValue",
                table: "Discounts");

            migrationBuilder.AlterColumn<int>(
                name: "Rate",
                table: "Taxes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Rate",
                table: "Discounts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiscountRate",
                table: "BranchItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RestockThreshold",
                table: "BranchItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DiscountSalesInvoices",
                columns: table => new
                {
                    DiscountId = table.Column<int>(type: "int", nullable: false),
                    OperationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountSalesInvoices", x => new { x.DiscountId, x.OperationId });
                    table.ForeignKey(
                        name: "FK_DiscountSalesInvoices_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscountSalesInvoices_SalesInvoices_OperationId",
                        column: x => x.OperationId,
                        principalTable: "SalesInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscountSalesInvoices_OperationId",
                table: "DiscountSalesInvoices",
                column: "OperationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscountSalesInvoices");

            migrationBuilder.DropColumn(
                name: "DiscountRate",
                table: "BranchItems");

            migrationBuilder.DropColumn(
                name: "RestockThreshold",
                table: "BranchItems");

            migrationBuilder.AlterColumn<int>(
                name: "Rate",
                table: "Taxes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "RawValue",
                table: "Taxes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiscountPercentage",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RestockThreshold",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaxPercentage",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Rate",
                table: "Discounts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "RawValue",
                table: "Discounts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DiscountOperations",
                columns: table => new
                {
                    DiscountId = table.Column<int>(type: "int", nullable: false),
                    OperationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountOperations", x => new { x.DiscountId, x.OperationId });
                    table.ForeignKey(
                        name: "FK_DiscountOperations_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscountOperations_Operations_OperationId",
                        column: x => x.OperationId,
                        principalTable: "Operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Taxes_RateOrRawValue",
                table: "Taxes",
                sql: "((Rate IS NULL OR Rate = 0) OR (RawValue IS NULL OR RawValue = 0)) AND NOT ((Rate IS NOT NULL AND Rate <> 0) AND (RawValue IS NOT NULL AND RawValue <> 0))");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Discounts_RateOrRawValue",
                table: "Discounts",
                sql: "((Rate IS NULL OR Rate = 0) OR (RawValue IS NULL OR RawValue = 0)) AND NOT ((Rate IS NOT NULL AND Rate <> 0) AND (RawValue IS NOT NULL AND RawValue <> 0))");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountOperations_OperationId",
                table: "DiscountOperations",
                column: "OperationId");
        }
    }
}
