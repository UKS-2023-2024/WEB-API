using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeleteCascadeOrganizationRepository : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Repositories_Organizations_OrganizationId",
                table: "Repositories");

            migrationBuilder.AddForeignKey(
                name: "FK_Repositories_Organizations_OrganizationId",
                table: "Repositories",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Repositories_Organizations_OrganizationId",
                table: "Repositories");

            migrationBuilder.AddForeignKey(
                name: "FK_Repositories_Organizations_OrganizationId",
                table: "Repositories",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id");
        }
    }
}
