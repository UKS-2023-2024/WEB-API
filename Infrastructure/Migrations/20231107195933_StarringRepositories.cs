using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StarringRepositories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RepositoryUser1",
                columns: table => new
                {
                    StarredById = table.Column<Guid>(type: "uuid", nullable: false),
                    StarredId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepositoryUser1", x => new { x.StarredById, x.StarredId });
                    table.ForeignKey(
                        name: "FK_RepositoryUser1_Repositories_StarredId",
                        column: x => x.StarredId,
                        principalTable: "Repositories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RepositoryUser1_Users_StarredById",
                        column: x => x.StarredById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RepositoryUser1_StarredId",
                table: "RepositoryUser1",
                column: "StarredId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RepositoryUser1");
        }
    }
}
