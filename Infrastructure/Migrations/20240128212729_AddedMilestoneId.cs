using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedMilestoneId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MilestoneId",
                table: "Events",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_MilestoneId",
                table: "Events",
                column: "MilestoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Milestones_MilestoneId",
                table: "Events",
                column: "MilestoneId",
                principalTable: "Milestones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Milestones_MilestoneId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_MilestoneId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MilestoneId",
                table: "Events");
        }
    }
}
