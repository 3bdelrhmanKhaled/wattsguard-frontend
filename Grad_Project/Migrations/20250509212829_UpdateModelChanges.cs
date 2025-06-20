using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grad_Project.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "picPublicId",
                table: "devices");

            migrationBuilder.DropColumn(
                name: "picUrl",
                table: "devices");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "picPublicId",
                table: "devices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "picUrl",
                table: "devices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
