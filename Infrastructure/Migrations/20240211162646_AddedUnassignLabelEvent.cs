using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedUnassignLabelEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UnassignLabelEvent_LabelId",
                table: "Events",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_UnassignLabelEvent_LabelId",
                table: "Events",
                column: "UnassignLabelEvent_LabelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Labels_UnassignLabelEvent_LabelId",
                table: "Events",
                column: "UnassignLabelEvent_LabelId",
                principalTable: "Labels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Labels_UnassignLabelEvent_LabelId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_UnassignLabelEvent_LabelId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "UnassignLabelEvent_LabelId",
                table: "Events");
        }
    }
}
