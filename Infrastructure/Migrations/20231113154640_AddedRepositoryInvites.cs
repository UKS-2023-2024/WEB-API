using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedRepositoryInvites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepositoryUser_Repositories_PendingRepositoriesId",
                table: "RepositoryUser");

            migrationBuilder.DropForeignKey(
                name: "FK_RepositoryUser_Users_PendingMembersId",
                table: "RepositoryUser");

            migrationBuilder.DropTable(
                name: "RepositoryUser1");

            migrationBuilder.RenameColumn(
                name: "PendingRepositoriesId",
                table: "RepositoryUser",
                newName: "StarredId");

            migrationBuilder.RenameColumn(
                name: "PendingMembersId",
                table: "RepositoryUser",
                newName: "StarredById");

            migrationBuilder.RenameIndex(
                name: "IX_RepositoryUser_PendingRepositoriesId",
                table: "RepositoryUser",
                newName: "IX_RepositoryUser_StarredId");

            migrationBuilder.CreateTable(
                name: "RepositoryInvites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RepositoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepositoryInvites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepositoryInvites_Repositories_RepositoryId",
                        column: x => x.RepositoryId,
                        principalTable: "Repositories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RepositoryInvites_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RepositoryInvites_RepositoryId_UserId",
                table: "RepositoryInvites",
                columns: new[] { "RepositoryId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RepositoryInvites_UserId",
                table: "RepositoryInvites",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RepositoryUser_Repositories_StarredId",
                table: "RepositoryUser",
                column: "StarredId",
                principalTable: "Repositories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RepositoryUser_Users_StarredById",
                table: "RepositoryUser",
                column: "StarredById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepositoryUser_Repositories_StarredId",
                table: "RepositoryUser");

            migrationBuilder.DropForeignKey(
                name: "FK_RepositoryUser_Users_StarredById",
                table: "RepositoryUser");

            migrationBuilder.DropTable(
                name: "RepositoryInvites");

            migrationBuilder.RenameColumn(
                name: "StarredId",
                table: "RepositoryUser",
                newName: "PendingRepositoriesId");

            migrationBuilder.RenameColumn(
                name: "StarredById",
                table: "RepositoryUser",
                newName: "PendingMembersId");

            migrationBuilder.RenameIndex(
                name: "IX_RepositoryUser_StarredId",
                table: "RepositoryUser",
                newName: "IX_RepositoryUser_PendingRepositoriesId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_RepositoryUser_Repositories_PendingRepositoriesId",
                table: "RepositoryUser",
                column: "PendingRepositoriesId",
                principalTable: "Repositories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RepositoryUser_Users_PendingMembersId",
                table: "RepositoryUser",
                column: "PendingMembersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
