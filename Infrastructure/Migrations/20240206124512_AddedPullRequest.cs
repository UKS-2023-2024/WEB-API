using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedPullRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssignEvent_AssigneeId",
                table: "Events",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UnnassignPullRequestEvent_AssigneeId",
                table: "Events",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedFrom",
                table: "Branches",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_AssignEvent_AssigneeId",
                table: "Events",
                column: "AssignEvent_AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_UnnassignPullRequestEvent_AssigneeId",
                table: "Events",
                column: "UnnassignPullRequestEvent_AssigneeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_RepositoryMembers_AssignEvent_AssigneeId",
                table: "Events",
                column: "AssignEvent_AssigneeId",
                principalTable: "RepositoryMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_RepositoryMembers_UnnassignPullRequestEvent_Assignee~",
                table: "Events",
                column: "UnnassignPullRequestEvent_AssigneeId",
                principalTable: "RepositoryMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_RepositoryMembers_AssignEvent_AssigneeId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_RepositoryMembers_UnnassignPullRequestEvent_Assignee~",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_AssignEvent_AssigneeId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_UnnassignPullRequestEvent_AssigneeId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "AssignEvent_AssigneeId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "UnnassignPullRequestEvent_AssigneeId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CreatedFrom",
                table: "Branches");
        }
    }
}
