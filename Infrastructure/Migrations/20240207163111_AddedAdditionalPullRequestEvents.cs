using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedAdditionalPullRequestEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FromBranchId",
                table: "Tasks",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GitPullRequestId",
                table: "Tasks",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PullRequestId",
                table: "Tasks",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ToBranchId",
                table: "Tasks",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IssueId",
                table: "Events",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RemoveIssueFromPullRequestEvent_IssueId",
                table: "Events",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_FromBranchId",
                table: "Tasks",
                column: "FromBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_PullRequestId",
                table: "Tasks",
                column: "PullRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ToBranchId",
                table: "Tasks",
                column: "ToBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_IssueId",
                table: "Events",
                column: "IssueId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_RemoveIssueFromPullRequestEvent_IssueId",
                table: "Events",
                column: "RemoveIssueFromPullRequestEvent_IssueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Tasks_IssueId",
                table: "Events",
                column: "IssueId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Tasks_RemoveIssueFromPullRequestEvent_IssueId",
                table: "Events",
                column: "RemoveIssueFromPullRequestEvent_IssueId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Branches_FromBranchId",
                table: "Tasks",
                column: "FromBranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Branches_ToBranchId",
                table: "Tasks",
                column: "ToBranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Tasks_PullRequestId",
                table: "Tasks",
                column: "PullRequestId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Tasks_IssueId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Tasks_RemoveIssueFromPullRequestEvent_IssueId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Branches_FromBranchId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Branches_ToBranchId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Tasks_PullRequestId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_FromBranchId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_PullRequestId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_ToBranchId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Events_IssueId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_RemoveIssueFromPullRequestEvent_IssueId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "FromBranchId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "GitPullRequestId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "PullRequestId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ToBranchId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "IssueId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "RemoveIssueFromPullRequestEvent_IssueId",
                table: "Events");
        }
    }
}
