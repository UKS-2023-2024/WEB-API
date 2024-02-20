using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedWatchedByToRepositoryEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RepositoryUser1");
        }
    }
}
