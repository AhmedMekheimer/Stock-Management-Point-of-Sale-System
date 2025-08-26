using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class EitherRateOrRawValue_CheckConstraint_Tax_Discount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Taxes_RateOrRawValue",
                table: "Taxes",
                sql: "((Rate IS NULL OR Rate = 0) OR (RawValue IS NULL OR RawValue = 0)) AND NOT ((Rate IS NOT NULL AND Rate <> 0) AND (RawValue IS NOT NULL AND RawValue <> 0))");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Discounts_RateOrRawValue",
                table: "Discounts",
                sql: "((Rate IS NULL OR Rate = 0) OR (RawValue IS NULL OR RawValue = 0)) AND NOT ((Rate IS NOT NULL AND Rate <> 0) AND (RawValue IS NOT NULL AND RawValue <> 0))");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Taxes_RateOrRawValue",
                table: "Taxes");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Discounts_RateOrRawValue",
                table: "Discounts");
        }
    }
}
