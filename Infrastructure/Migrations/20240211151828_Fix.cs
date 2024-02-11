using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Labels_Title_RepositoryId",
                table: "Labels");

            migrationBuilder.CreateIndex(
                name: "IX_Labels_Title_RepositoryId_IsDefaultLabel",
                table: "Labels",
                columns: new[] { "Title", "RepositoryId", "IsDefaultLabel" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Labels_Title_RepositoryId_IsDefaultLabel",
                table: "Labels");

            migrationBuilder.CreateIndex(
                name: "IX_Labels_Title_RepositoryId",
                table: "Labels",
                columns: new[] { "Title", "RepositoryId" },
                unique: true);
        }
    }
}
