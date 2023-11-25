using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedOwnerFieldToBranchEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Branches",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Branches_OwnerId",
                table: "Branches",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_Users_OwnerId",
                table: "Branches",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Users_OwnerId",
                table: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_Branches_OwnerId",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Branches");
        }
    }
}
