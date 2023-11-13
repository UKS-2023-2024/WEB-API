using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovedInviteOrganizationMemberConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizationInvites_OrganizationMembers_OrganizationMemberO~",
                table: "OrganizationInvites");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationInvites_OrganizationMemberOrganizationId_Organi~",
                table: "OrganizationInvites");

            migrationBuilder.DropColumn(
                name: "OrganizationMemberMemberId",
                table: "OrganizationInvites");

            migrationBuilder.DropColumn(
                name: "OrganizationMemberOrganizationId",
                table: "OrganizationInvites");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationInvites_OrganizationMemberOrganizationId_Organi~",
                table: "OrganizationInvites",
                columns: new[] { "OrganizationMemberOrganizationId", "OrganizationMemberMemberId" });

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizationInvites_OrganizationMembers_OrganizationMemberO~",
                table: "OrganizationInvites",
                columns: new[] { "OrganizationMemberOrganizationId", "OrganizationMemberMemberId" },
                principalTable: "OrganizationMembers",
                principalColumns: new[] { "OrganizationId", "MemberId" });
        }
    }
}
