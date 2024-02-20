using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedUnassignMilestoneId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UnassignMilestoneEvent_MilestoneId",
                table: "Events",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_UnassignMilestoneEvent_MilestoneId",
                table: "Events",
                column: "UnassignMilestoneEvent_MilestoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Milestones_UnassignMilestoneEvent_MilestoneId",
                table: "Events",
                column: "UnassignMilestoneEvent_MilestoneId",
                principalTable: "Milestones",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Milestones_UnassignMilestoneEvent_MilestoneId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_UnassignMilestoneEvent_MilestoneId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "UnassignMilestoneEvent_MilestoneId",
                table: "Events");
        }
    }
}
