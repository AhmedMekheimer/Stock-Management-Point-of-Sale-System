using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddedLastBuyingPrice_BuyingPriceAvg_ToBranchItemTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyingPrice",
                table: "BranchItems");

            migrationBuilder.AlterColumn<double>(
                name: "SellingPrice",
                table: "BranchItems",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BuyingPriceAvg",
                table: "BranchItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "lastBuyingPrice",
                table: "BranchItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyingPriceAvg",
                table: "BranchItems");

            migrationBuilder.DropColumn(
                name: "lastBuyingPrice",
                table: "BranchItems");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingPrice",
                table: "BranchItems",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BuyingPrice",
                table: "BranchItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
