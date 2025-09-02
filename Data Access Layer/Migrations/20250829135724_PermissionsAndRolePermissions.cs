using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InfrastructureLayer.Migrations
{
    /// <inheritdoc />
    public partial class PermissionsAndRolePermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "lastBuyingPrice",
                table: "BranchItems",
                newName: "LastBuyingPrice");

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    EnglishName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permissions_Permissions_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Permissions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "EnglishName", "Name", "ParentId" },
                values: new object[,]
                {
                    { 1, "System", "System", null },
                    { 10, "Branch", "Branch", 1 },
                    { 11, "Item", "Item", 1 },
                    { 12, "Administrative", "Administrative", 1 },
                    { 100, "Stock", "Stock", 10 },
                    { 120, "Receive Order", "ReceiveOrder", 10 },
                    { 140, "Clothing ClothingItem", "ClothingItem", 11 },
                    { 160, "Color", "Color", 11 },
                    { 180, "Size", "Size", 11 },
                    { 200, "Item Type", "ItemType", 11 },
                    { 220, "Target Audience", "TargetAudience", 11 },
                    { 240, "Brand", "Brand", 11 },
                    { 260, "Partner", "Partner", 12 },
                    { 280, "User", "User", 12 },
                    { 300, "Role", "Role", 12 },
                    { 320, "Setting", "Setting", 12 },
                    { 101, "View Stock", "Stock.View", 100 },
                    { 102, "Add Stock", "Stock.Add", 100 },
                    { 103, "Edit Stock", "Stock.Edit", 100 },
                    { 104, "Delete Stock", "Stock.Delete", 100 },
                    { 121, "View Receive Order", "ReceiveOrder.View", 120 },
                    { 122, "Add Receive Order", "ReceiveOrder.Add", 120 },
                    { 123, "Edit Receive Order", "ReceiveOrder.Edit", 120 },
                    { 124, "Delete Receive Order", "ReceiveOrder.Delete", 120 },
                    { 141, "View ClothingItem", "ClothingItem.View", 140 },
                    { 142, "Add ClothingItem", "ClothingItem.Add", 140 },
                    { 143, "Edit ClothingItem", "ClothingItem.Edit", 140 },
                    { 144, "Delete ClothingItem", "ClothingItem.Delete", 140 },
                    { 161, "View Color", "Color.View", 160 },
                    { 162, "Add Color", "Color.Add", 160 },
                    { 163, "Edit Color", "Color.Edit", 160 },
                    { 164, "Delete Color", "Color.Delete", 160 },
                    { 181, "View Size", "Size.View", 180 },
                    { 182, "Add Size", "Size.Add", 180 },
                    { 183, "Edit Size", "Size.Edit", 180 },
                    { 184, "Delete Size", "Size.Delete", 180 },
                    { 201, "View Item Type", "ItemType.View", 200 },
                    { 202, "Add Item Type", "ItemType.Add", 200 },
                    { 203, "Edit Item Type", "ItemType.Edit", 200 },
                    { 204, "Delete Item Type", "ItemType.Delete", 200 },
                    { 221, "View Target Audience", "TargetAudience.View", 220 },
                    { 222, "Add Target Audience", "TargetAudience.Add", 220 },
                    { 223, "Edit Target Audience", "TargetAudience.Edit", 220 },
                    { 224, "Delete Target Audience", "TargetAudience.Delete", 220 },
                    { 241, "View Brand", "Brand.View", 240 },
                    { 242, "Add Brand", "Brand.Add", 240 },
                    { 243, "Edit Brand", "Brand.Edit", 240 },
                    { 244, "Delete Brand", "Brand.Delete", 240 },
                    { 261, "View Partner", "Partner.View", 260 },
                    { 262, "Add Partner", "Partner.Add", 260 },
                    { 263, "Edit Partner", "Partner.Edit", 260 },
                    { 264, "Delete Partner", "Partner.Delete", 260 },
                    { 281, "View User", "User.View", 280 },
                    { 282, "Add User", "User.Add", 280 },
                    { 283, "Edit User", "User.Edit", 280 },
                    { 284, "Delete User", "User.Delete", 280 },
                    { 301, "View Role", "Role.View", 300 },
                    { 302, "Add Role", "Role.Add", 300 },
                    { 303, "Edit Role", "Role.Edit", 300 },
                    { 304, "Delete Role", "Role.Delete", 300 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_ParentId",
                table: "Permissions",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.RenameColumn(
                name: "LastBuyingPrice",
                table: "BranchItems",
                newName: "lastBuyingPrice");
        }
    }
}
