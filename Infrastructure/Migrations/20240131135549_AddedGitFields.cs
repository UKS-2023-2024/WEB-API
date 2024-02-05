using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedGitFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GitToken",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HttpCloneUrl",
                table: "Repositories",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SshCloneUrl",
                table: "Repositories",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GitToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "HttpCloneUrl",
                table: "Repositories");

            migrationBuilder.DropColumn(
                name: "SshCloneUrl",
                table: "Repositories");
        }
    }
}
