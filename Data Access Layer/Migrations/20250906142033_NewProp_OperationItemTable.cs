using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class NewProp_OperationItemTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalDiscountRateOrRawValue",
                table: "Operations",
                newName: "TotalDiscountRate");

            migrationBuilder.AddColumn<int>(
                name: "DiscountRate",
                table: "OperationItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ItemNameSnapshot",
                table: "OperationItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "SellingPrice",
                table: "OperationItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountRate",
                table: "OperationItems");

            migrationBuilder.DropColumn(
                name: "ItemNameSnapshot",
                table: "OperationItems");

            migrationBuilder.DropColumn(
                name: "SellingPrice",
                table: "OperationItems");

            migrationBuilder.RenameColumn(
                name: "TotalDiscountRate",
                table: "Operations",
                newName: "TotalDiscountRateOrRawValue");
        }
    }
}
