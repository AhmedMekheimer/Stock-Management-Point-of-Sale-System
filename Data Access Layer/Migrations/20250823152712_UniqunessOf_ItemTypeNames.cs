using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class UniqunessOf_ItemTypeNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemTypes_ItemTypeId",
                table: "ItemTypes");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ItemTypes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTypes_ItemTypeId_Name",
                table: "ItemTypes",
                columns: new[] { "ItemTypeId", "Name" },
                unique: true,
                filter: "[ItemTypeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTypes_Name",
                table: "ItemTypes",
                column: "Name",
                unique: true,
                filter: "[ItemTypeId] IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemTypes_ItemTypeId_Name",
                table: "ItemTypes");

            migrationBuilder.DropIndex(
                name: "IX_ItemTypes_Name",
                table: "ItemTypes");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ItemTypes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTypes_ItemTypeId",
                table: "ItemTypes",
                column: "ItemTypeId");
        }
    }
}
