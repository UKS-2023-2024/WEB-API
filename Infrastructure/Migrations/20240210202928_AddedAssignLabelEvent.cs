using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedAssignLabelEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LabelId",
                table: "Events",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_LabelId",
                table: "Events",
                column: "LabelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Labels_LabelId",
                table: "Events",
                column: "LabelId",
                principalTable: "Labels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Labels_LabelId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_LabelId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "LabelId",
                table: "Events");
        }
    }
}
