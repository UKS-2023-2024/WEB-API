using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedConnectionWithIssueAndPullRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Tasks_PullRequestId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_PullRequestId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "PullRequestId",
                table: "Tasks");

            migrationBuilder.CreateTable(
                name: "IssuePullRequest",
                columns: table => new
                {
                    IssuesId = table.Column<Guid>(type: "uuid", nullable: false),
                    PullRequestsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssuePullRequest", x => new { x.IssuesId, x.PullRequestsId });
                    table.ForeignKey(
                        name: "FK_IssuePullRequest_Tasks_IssuesId",
                        column: x => x.IssuesId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IssuePullRequest_Tasks_PullRequestsId",
                        column: x => x.PullRequestsId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IssuePullRequest_PullRequestsId",
                table: "IssuePullRequest",
                column: "PullRequestsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IssuePullRequest");

            migrationBuilder.AddColumn<Guid>(
                name: "PullRequestId",
                table: "Tasks",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_PullRequestId",
                table: "Tasks",
                column: "PullRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Tasks_PullRequestId",
                table: "Tasks",
                column: "PullRequestId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }
    }
}
