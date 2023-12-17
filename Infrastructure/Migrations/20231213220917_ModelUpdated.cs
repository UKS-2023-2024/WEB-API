using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModelUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssigneeId",
                table: "Events",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UnassignEvent_AssigneeId",
                table: "Events",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_AssigneeId",
                table: "Events",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_UnassignEvent_AssigneeId",
                table: "Events",
                column: "UnassignEvent_AssigneeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_RepositoryMembers_AssigneeId",
                table: "Events",
                column: "AssigneeId",
                principalTable: "RepositoryMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_RepositoryMembers_UnassignEvent_AssigneeId",
                table: "Events",
                column: "UnassignEvent_AssigneeId",
                principalTable: "RepositoryMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_RepositoryMembers_AssigneeId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_RepositoryMembers_UnassignEvent_AssigneeId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_AssigneeId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_UnassignEvent_AssigneeId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "AssigneeId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "UnassignEvent_AssigneeId",
                table: "Events");
        }
    }
}
