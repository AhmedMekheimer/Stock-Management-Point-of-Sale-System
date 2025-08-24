using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class RetailCustomerNotRequired_ItemVariantsDeleteRestriction_RemovedInvoiceTable_AddedImageForItemVariants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Brands_BrandId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Colors_ColorId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemTypes_ItemTypeId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Sizes_SizeId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_TargetAudiences_TargetAudienceId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationItems_Items_ItemId",
                table: "OperationItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesInvoices_Partners_CorporateCustomerId",
                table: "SalesInvoices");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_SalesInvoices_CorporateCustomerId",
                table: "SalesInvoices");

            migrationBuilder.DropColumn(
                name: "CorporateCustomerId",
                table: "SalesInvoices");

            migrationBuilder.AddColumn<DateOnly>(
                name: "ExpirationDate",
                table: "Vouchers",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaximumUses",
                table: "Vouchers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "TargetAudiences",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Sizes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RetailCustomerId",
                table: "SalesInvoices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VoucherId",
                table: "SalesInvoices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "ItemTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Colors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoices_RetailCustomerId",
                table: "SalesInvoices",
                column: "RetailCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoices_VoucherId",
                table: "SalesInvoices",
                column: "VoucherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Brands_BrandId",
                table: "Items",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Colors_ColorId",
                table: "Items",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemTypes_ItemTypeId",
                table: "Items",
                column: "ItemTypeId",
                principalTable: "ItemTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Sizes_SizeId",
                table: "Items",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_TargetAudiences_TargetAudienceId",
                table: "Items",
                column: "TargetAudienceId",
                principalTable: "TargetAudiences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OperationItems_Items_ItemId",
                table: "OperationItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoices_Partners_RetailCustomerId",
                table: "SalesInvoices",
                column: "RetailCustomerId",
                principalTable: "Partners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoices_Vouchers_VoucherId",
                table: "SalesInvoices",
                column: "VoucherId",
                principalTable: "Vouchers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Brands_BrandId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Colors_ColorId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemTypes_ItemTypeId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Sizes_SizeId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_TargetAudiences_TargetAudienceId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationItems_Items_ItemId",
                table: "OperationItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesInvoices_Partners_RetailCustomerId",
                table: "SalesInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesInvoices_Vouchers_VoucherId",
                table: "SalesInvoices");

            migrationBuilder.DropIndex(
                name: "IX_SalesInvoices_RetailCustomerId",
                table: "SalesInvoices");

            migrationBuilder.DropIndex(
                name: "IX_SalesInvoices_VoucherId",
                table: "SalesInvoices");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "MaximumUses",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "TargetAudiences");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Sizes");

            migrationBuilder.DropColumn(
                name: "RetailCustomerId",
                table: "SalesInvoices");

            migrationBuilder.DropColumn(
                name: "VoucherId",
                table: "SalesInvoices");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "ItemTypes");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Colors");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Brands");

            migrationBuilder.AddColumn<int>(
                name: "CorporateCustomerId",
                table: "SalesInvoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    RetailCustomerId = table.Column<int>(type: "int", nullable: false),
                    VoucherId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Branches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoices_Operations_Id",
                        column: x => x.Id,
                        principalTable: "Operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoices_Partners_RetailCustomerId",
                        column: x => x.RetailCustomerId,
                        principalTable: "Partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invoices_Vouchers_VoucherId",
                        column: x => x.VoucherId,
                        principalTable: "Vouchers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoices_CorporateCustomerId",
                table: "SalesInvoices",
                column: "CorporateCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_BranchId",
                table: "Invoices",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_RetailCustomerId",
                table: "Invoices",
                column: "RetailCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_VoucherId",
                table: "Invoices",
                column: "VoucherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Brands_BrandId",
                table: "Items",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Colors_ColorId",
                table: "Items",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemTypes_ItemTypeId",
                table: "Items",
                column: "ItemTypeId",
                principalTable: "ItemTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Sizes_SizeId",
                table: "Items",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_TargetAudiences_TargetAudienceId",
                table: "Items",
                column: "TargetAudienceId",
                principalTable: "TargetAudiences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OperationItems_Items_ItemId",
                table: "OperationItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoices_Partners_CorporateCustomerId",
                table: "SalesInvoices",
                column: "CorporateCustomerId",
                principalTable: "Partners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
