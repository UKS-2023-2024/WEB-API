using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedOrgMemberInvite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizationMembers",
                table: "OrganizationMembers");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationMembers_OrganizationId",
                table: "OrganizationMembers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizationMembers",
                table: "OrganizationMembers",
                columns: new[] { "OrganizationId", "MemberId" });

            migrationBuilder.CreateTable(
                name: "OrganizationInvites",
                columns: table => new
                {
                    MemberId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationInvites", x => new { x.OrganizationId, x.MemberId, x.Token });
                    table.ForeignKey(
                        name: "FK_OrganizationInvites_OrganizationMembers_OrganizationId_Memb~",
                        columns: x => new { x.OrganizationId, x.MemberId },
                        principalTable: "OrganizationMembers",
                        principalColumns: new[] { "OrganizationId", "MemberId" },
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganizationInvites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizationMembers",
                table: "OrganizationMembers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizationMembers",
                table: "OrganizationMembers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationMembers_OrganizationId",
                table: "OrganizationMembers",
                column: "OrganizationId");
        }
    }
}
