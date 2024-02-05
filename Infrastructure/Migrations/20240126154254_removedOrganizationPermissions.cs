using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removedOrganizationPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationMembers_OrganizationRoles_RoleName",
                table: "OrganizationMembers");

            migrationBuilder.DropTable(
                name: "OrganizationRolePermissions");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "IX_OrganizationMembers_RoleName",
                table: "OrganizationMembers",
                column: "RoleName");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationRolePermissions_PermissionName",
                table: "OrganizationRolePermissions",
                column: "PermissionName");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationMembers_OrganizationRoles_RoleName",
                table: "OrganizationMembers",
                column: "RoleName",
                principalTable: "OrganizationRoles",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
