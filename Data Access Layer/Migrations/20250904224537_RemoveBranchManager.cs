using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBranchManager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_AspNetUsers_BranchManagerId",
                table: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_Branches_BranchManagerId",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "BranchManagerId",
                table: "Branches");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BranchManagerId",
                table: "Branches",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_BranchManagerId",
                table: "Branches",
                column: "BranchManagerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_AspNetUsers_BranchManagerId",
                table: "Branches",
                column: "BranchManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
