using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobileOperatorAppServer.Migrations
{
    /// <inheritdoc />
    public partial class add_services_to_user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserModelId",
                table: "Services",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_UserModelId",
                table: "Services",
                column: "UserModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Users_UserModelId",
                table: "Services",
                column: "UserModelId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Users_UserModelId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_UserModelId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "UserModelId",
                table: "Services");
        }
    }
}
