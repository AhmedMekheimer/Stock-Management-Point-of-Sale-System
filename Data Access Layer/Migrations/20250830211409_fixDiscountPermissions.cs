using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class fixDiscountPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 360,
                columns: new[] { "EnglishName", "Name" },
                values: new object[] { "Discount", "Discount" });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 361,
                columns: new[] { "EnglishName", "Name" },
                values: new object[] { "View Discount", "Discount.View" });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 362,
                columns: new[] { "EnglishName", "Name" },
                values: new object[] { "Add Discount", "Discount.Add" });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 363,
                columns: new[] { "EnglishName", "Name" },
                values: new object[] { "Edit Discount", "Discount.Edit" });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 364,
                columns: new[] { "EnglishName", "Name" },
                values: new object[] { "Delete Discount", "Discount.Delete" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 360,
                columns: new[] { "EnglishName", "Name" },
                values: new object[] { "Tax", "Tax" });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 361,
                columns: new[] { "EnglishName", "Name" },
                values: new object[] { "View Tax", "Tax.View" });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 362,
                columns: new[] { "EnglishName", "Name" },
                values: new object[] { "Add Tax", "Tax.Add" });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 363,
                columns: new[] { "EnglishName", "Name" },
                values: new object[] { "Edit Tax", "Tax.Edit" });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 364,
                columns: new[] { "EnglishName", "Name" },
                values: new object[] { "Delete Tax", "Tax.Delete" });
        }
    }
}
