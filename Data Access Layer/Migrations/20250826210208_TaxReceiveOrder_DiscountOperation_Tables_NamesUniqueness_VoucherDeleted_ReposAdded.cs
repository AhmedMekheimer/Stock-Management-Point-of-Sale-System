using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class TaxReceiveOrder_DiscountOperation_Tables_NamesUniqueness_VoucherDeleted_ReposAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesInvoices_Vouchers_VoucherId",
                table: "SalesInvoices");

            migrationBuilder.DropTable(
                name: "Vouchers");

            migrationBuilder.DropIndex(
                name: "IX_SalesInvoices_VoucherId",
                table: "SalesInvoices");

            migrationBuilder.DropColumn(
                name: "VoucherId",
                table: "SalesInvoices");

            migrationBuilder.RenameColumn(
                name: "TotalTaxes",
                table: "Operations",
                newName: "TotalTaxesRate");

            migrationBuilder.RenameColumn(
                name: "TotalItemsPrice",
                table: "Operations",
                newName: "TotalTaxesAmount");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "Operations",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "RoundedGrandTotal",
                table: "Operations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "Time",
                table: "Operations",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<double>(
                name: "TotalAmount",
                table: "Operations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalDiscountAmount",
                table: "Operations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalDiscountRate",
                table: "Operations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "TotalQuantity",
                table: "Operations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "Operations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Rate = table.Column<int>(type: "int", nullable: true),
                    RawValue = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ExpirationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    MaximumUses = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Taxes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Rate = table.Column<int>(type: "int", nullable: true),
                    RawValue = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxes", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "TaxReceiveOrders",
                columns: table => new
                {
                    TaxId = table.Column<int>(type: "int", nullable: false),
                    OperationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxReceiveOrders", x => new { x.TaxId, x.OperationId });
                    table.ForeignKey(
                        name: "FK_TaxReceiveOrders_ReceiveOrders_OperationId",
                        column: x => x.OperationId,
                        principalTable: "ReceiveOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaxReceiveOrders_Taxes_TaxId",
                        column: x => x.TaxId,
                        principalTable: "Taxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscountOperations_OperationId",
                table: "DiscountOperations",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_Name",
                table: "Discounts",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Taxes_Name",
                table: "Taxes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaxReceiveOrders_OperationId",
                table: "TaxReceiveOrders",
                column: "OperationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscountOperations");

            migrationBuilder.DropTable(
                name: "TaxReceiveOrders");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "Taxes");

            migrationBuilder.DropColumn(
                name: "RoundedGrandTotal",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "TotalDiscountAmount",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "TotalDiscountRate",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "TotalQuantity",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Operations");

            migrationBuilder.RenameColumn(
                name: "TotalTaxesRate",
                table: "Operations",
                newName: "TotalTaxes");

            migrationBuilder.RenameColumn(
                name: "TotalTaxesAmount",
                table: "Operations",
                newName: "TotalItemsPrice");

            migrationBuilder.AddColumn<int>(
                name: "VoucherId",
                table: "SalesInvoices",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Operations",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.CreateTable(
                name: "Vouchers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountValue = table.Column<double>(type: "float", nullable: false),
                    ExpirationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    MaximumUses = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouchers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoices_VoucherId",
                table: "SalesInvoices",
                column: "VoucherId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoices_Vouchers_VoucherId",
                table: "SalesInvoices",
                column: "VoucherId",
                principalTable: "Vouchers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
