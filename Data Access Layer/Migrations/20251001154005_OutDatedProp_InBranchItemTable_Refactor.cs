using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class OutDatedProp_InBranchItemTable_Refactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OutDated",
                table: "BranchItems");

            migrationBuilder.AddColumn<int>(
                name: "OutDatedInMonths",
                table: "BranchItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OutDatedInMonths",
                table: "BranchItems");

            migrationBuilder.AddColumn<DateOnly>(
                name: "OutDated",
                table: "BranchItems",
                type: "date",
                nullable: true);
        }
    }
}
