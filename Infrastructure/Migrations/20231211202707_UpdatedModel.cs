using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RepositoryId",
                table: "Labels",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Labels_RepositoryId",
                table: "Labels",
                column: "RepositoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Labels_Title_RepositoryId",
                table: "Labels",
                columns: new[] { "Title", "RepositoryId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Labels_Repositories_RepositoryId",
                table: "Labels",
                column: "RepositoryId",
                principalTable: "Repositories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Labels_Repositories_RepositoryId",
                table: "Labels");

            migrationBuilder.DropIndex(
                name: "IX_Labels_RepositoryId",
                table: "Labels");

            migrationBuilder.DropIndex(
                name: "IX_Labels_Title_RepositoryId",
                table: "Labels");

            migrationBuilder.DropColumn(
                name: "RepositoryId",
                table: "Labels");
        }
    }
}
