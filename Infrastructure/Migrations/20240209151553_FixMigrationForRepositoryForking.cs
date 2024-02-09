using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixMigrationForRepositoryForking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForkedFromId",
                table: "Repositories");

            migrationBuilder.CreateTable(
                name: "RepositoryForks",
                columns: table => new
                {
                    ForkedRepoId = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceRepoId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepositoryForks", x => new { x.SourceRepoId, x.ForkedRepoId });
                    table.ForeignKey(
                        name: "FK_RepositoryForks_Repositories_ForkedRepoId",
                        column: x => x.ForkedRepoId,
                        principalTable: "Repositories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RepositoryForks_Repositories_SourceRepoId",
                        column: x => x.SourceRepoId,
                        principalTable: "Repositories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RepositoryForks_ForkedRepoId",
                table: "RepositoryForks",
                column: "ForkedRepoId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RepositoryForks");

            migrationBuilder.AddColumn<Guid>(
                name: "ForkedFromId",
                table: "Repositories",
                type: "uuid",
                nullable: true);
        }
    }
}
