using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovedInviteMemberConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationInvites_OrganizationMembers_OrganizationId_Memb~",
                table: "OrganizationInvites");

            migrationBuilder.DropTable(
                name: "OrganizationUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizationInvites",
                table: "OrganizationInvites");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "OrganizationInvites");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "OrganizationInvites",
                newName: "UserId");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "OrganizationInvites",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationMemberMemberId",
                table: "OrganizationInvites",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationMemberOrganizationId",
                table: "OrganizationInvites",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizationInvites",
                table: "OrganizationInvites",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationInvites_OrganizationId_UserId",
                table: "OrganizationInvites",
                columns: new[] { "OrganizationId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationInvites_OrganizationMemberOrganizationId_Organi~",
                table: "OrganizationInvites",
                columns: new[] { "OrganizationMemberOrganizationId", "OrganizationMemberMemberId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationInvites_UserId",
                table: "OrganizationInvites",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationInvites_OrganizationMembers_OrganizationMemberO~",
                table: "OrganizationInvites",
                columns: new[] { "OrganizationMemberOrganizationId", "OrganizationMemberMemberId" },
                principalTable: "OrganizationMembers",
                principalColumns: new[] { "OrganizationId", "MemberId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationInvites_Organizations_OrganizationId",
                table: "OrganizationInvites",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationInvites_Users_UserId",
                table: "OrganizationInvites",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationInvites_OrganizationMembers_OrganizationMemberO~",
                table: "OrganizationInvites");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationInvites_Organizations_OrganizationId",
                table: "OrganizationInvites");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationInvites_Users_UserId",
                table: "OrganizationInvites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizationInvites",
                table: "OrganizationInvites");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationInvites_OrganizationId_UserId",
                table: "OrganizationInvites");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationInvites_OrganizationMemberOrganizationId_Organi~",
                table: "OrganizationInvites");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationInvites_UserId",
                table: "OrganizationInvites");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OrganizationInvites");

            migrationBuilder.DropColumn(
                name: "OrganizationMemberMemberId",
                table: "OrganizationInvites");

            migrationBuilder.DropColumn(
                name: "OrganizationMemberOrganizationId",
                table: "OrganizationInvites");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "OrganizationInvites",
                newName: "MemberId");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "OrganizationInvites",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizationInvites",
                table: "OrganizationInvites",
                columns: new[] { "OrganizationId", "MemberId", "Token" });

            migrationBuilder.CreateTable(
                name: "OrganizationUser",
                columns: table => new
                {
                    PendingMembersId = table.Column<Guid>(type: "uuid", nullable: false),
                    PendingOrganizationsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationUser", x => new { x.PendingMembersId, x.PendingOrganizationsId });
                    table.ForeignKey(
                        name: "FK_OrganizationUser_Organizations_PendingOrganizationsId",
                        column: x => x.PendingOrganizationsId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationUser_Users_PendingMembersId",
                        column: x => x.PendingMembersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUser_PendingOrganizationsId",
                table: "OrganizationUser",
                column: "PendingOrganizationsId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationInvites_OrganizationMembers_OrganizationId_Memb~",
                table: "OrganizationInvites",
                columns: new[] { "OrganizationId", "MemberId" },
                principalTable: "OrganizationMembers",
                principalColumns: new[] { "OrganizationId", "MemberId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
