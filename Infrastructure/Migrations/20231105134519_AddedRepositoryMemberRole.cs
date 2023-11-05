using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedRepositoryMemberRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Repositories_Organizations_OrganizationId",
                table: "Repositories");

            migrationBuilder.DropForeignKey(
                name: "FK_Repositories_Users_OwnerId",
                table: "Repositories");

            migrationBuilder.DropIndex(
                name: "IX_Repositories_OwnerId",
                table: "Repositories");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Repositories");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "RepositoryMembers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganizationId",
                table: "Repositories",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Repositories_Organizations_OrganizationId",
                table: "Repositories",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Repositories_Organizations_OrganizationId",
                table: "Repositories");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "RepositoryMembers");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganizationId",
                table: "Repositories",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Repositories",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Repositories_OwnerId",
                table: "Repositories",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Repositories_Organizations_OrganizationId",
                table: "Repositories",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Repositories_Users_OwnerId",
                table: "Repositories",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
