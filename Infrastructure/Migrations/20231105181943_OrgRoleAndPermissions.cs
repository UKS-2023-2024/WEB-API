using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OrgRoleAndPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "OrganizationMembers");

            migrationBuilder.AddColumn<string>(
                name: "RoleName",
                table: "OrganizationMembers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "OrganizationPermissions",
                columns: table => new
                {
                    Value = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationPermissions", x => x.Value);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationRoles",
                columns: table => new
                {
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationRoles", x => x.Name);
                });

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
                name: "IX_OrganizationMembers_RoleName",
                table: "OrganizationMembers",
                column: "RoleName");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationPermissionOrganizationRole_RolesName",
                table: "OrganizationPermissionOrganizationRole",
                column: "RolesName");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationMembers_OrganizationRoles_RoleName",
                table: "OrganizationMembers",
                column: "RoleName",
                principalTable: "OrganizationRoles",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationMembers_OrganizationRoles_RoleName",
                table: "OrganizationMembers");

            migrationBuilder.DropTable(
                name: "OrganizationPermissionOrganizationRole");

            migrationBuilder.DropTable(
                name: "OrganizationPermissions");

            migrationBuilder.DropTable(
                name: "OrganizationRoles");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationMembers_RoleName",
                table: "OrganizationMembers");

            migrationBuilder.DropColumn(
                name: "RoleName",
                table: "OrganizationMembers");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "OrganizationMembers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
