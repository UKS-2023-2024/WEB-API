using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedRepositoryWatcherEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RepositoryUser1");

            migrationBuilder.CreateTable(
                name: "RepositoryWatchers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RepositoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    WatchingPreferences = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepositoryWatchers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepositoryWatchers_Repositories_RepositoryId",
                        column: x => x.RepositoryId,
                        principalTable: "Repositories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RepositoryWatchers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RepositoryWatchers_RepositoryId",
                table: "RepositoryWatchers",
                column: "RepositoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RepositoryWatchers_UserId",
                table: "RepositoryWatchers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RepositoryWatchers");

            migrationBuilder.CreateTable(
                name: "RepositoryUser1",
                columns: table => new
                {
                    WatchedById = table.Column<Guid>(type: "uuid", nullable: false),
                    WatchedId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepositoryUser1", x => new { x.WatchedById, x.WatchedId });
                    table.ForeignKey(
                        name: "FK_RepositoryUser1_Repositories_WatchedId",
                        column: x => x.WatchedId,
                        principalTable: "Repositories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RepositoryUser1_Users_WatchedById",
                        column: x => x.WatchedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RepositoryUser1_WatchedId",
                table: "RepositoryUser1",
                column: "WatchedId");
        }
    }
}
