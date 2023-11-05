using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OrgRoleAndPermissionSeeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganizationPermissionOrganizationRole");

            migrationBuilder.CreateTable(
                name: "OrganizationRolePermissions",
                columns: table => new
                {
                    RoleName = table.Column<string>(type: "text", nullable: false),
                    PermissionName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationRolePermissions", x => new { x.RoleName, x.PermissionName });
                    table.ForeignKey(
                        name: "FK_OrganizationRolePermissions_OrganizationPermissions_Permiss~",
                        column: x => x.PermissionName,
                        principalTable: "OrganizationPermissions",
                        principalColumn: "Value",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationRolePermissions_OrganizationRoles_RoleName",
                        column: x => x.RoleName,
                        principalTable: "OrganizationRoles",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "OrganizationPermissions",
                columns: new[] { "Value", "Description" },
                values: new object[,]
                {
                    { "admin", "" },
                    { "manager", "" },
                    { "owner", "" },
                    { "read_only", "" }
                });

            migrationBuilder.InsertData(
                table: "OrganizationRoles",
                columns: new[] { "Name", "Description" },
                values: new object[,]
                {
                    { "MEMBER", "Member has all rights except owners" },
                    { "OWNER", "Has all rights!" }
                });

            migrationBuilder.InsertData(
                table: "OrganizationRolePermissions",
                columns: new[] { "PermissionName", "RoleName" },
                values: new object[,]
                {
                    { "manager", "MEMBER" },
                    { "read_only", "MEMBER" },
                    { "admin", "OWNER" },
                    { "manager", "OWNER" },
                    { "owner", "OWNER" },
                    { "read_only", "OWNER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationRolePermissions_PermissionName",
                table: "OrganizationRolePermissions",
                column: "PermissionName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganizationRolePermissions");

            migrationBuilder.DeleteData(
                table: "OrganizationPermissions",
                keyColumn: "Value",
                keyValue: "admin");

            migrationBuilder.DeleteData(
                table: "OrganizationPermissions",
                keyColumn: "Value",
                keyValue: "manager");

            migrationBuilder.DeleteData(
                table: "OrganizationPermissions",
                keyColumn: "Value",
                keyValue: "owner");

            migrationBuilder.DeleteData(
                table: "OrganizationPermissions",
                keyColumn: "Value",
                keyValue: "read_only");

            migrationBuilder.DeleteData(
                table: "OrganizationRoles",
                keyColumn: "Name",
                keyValue: "MEMBER");

            migrationBuilder.DeleteData(
                table: "OrganizationRoles",
                keyColumn: "Name",
                keyValue: "OWNER");

            migrationBuilder.CreateTable(
                name: "OrganizationPermissionOrganizationRole",
                columns: table => new
                {
                    PermissionsValue = table.Column<string>(type: "text", nullable: false),
                    RolesName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationPermissionOrganizationRole", x => new { x.PermissionsValue, x.RolesName });
                    table.ForeignKey(
                        name: "FK_OrganizationPermissionOrganizationRole_OrganizationPermissi~",
                        column: x => x.PermissionsValue,
                        principalTable: "OrganizationPermissions",
                        principalColumn: "Value",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationPermissionOrganizationRole_OrganizationRoles_Ro~",
                        column: x => x.RolesName,
                        principalTable: "OrganizationRoles",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationPermissionOrganizationRole_RolesName",
                table: "OrganizationPermissionOrganizationRole",
                column: "RolesName");
        }
    }
}
