using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class Removed_ItemNameSnap_RestockThreshold : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemNameSnapshot",
                table: "OperationItems");

            migrationBuilder.DropColumn(
                name: "RestockThreshold",
                table: "BranchItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ItemNameSnapshot",
                table: "OperationItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RestockThreshold",
                table: "BranchItems",
                type: "int",
                nullable: true);
        }
    }
}
