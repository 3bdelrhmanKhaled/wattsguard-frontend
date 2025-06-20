using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grad_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintToCounterId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CounterId",
                table: "counters",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_counters_CounterId",
                table: "counters",
                column: "CounterId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_counters_CounterId",
                table: "counters");

            migrationBuilder.AlterColumn<string>(
                name: "CounterId",
                table: "counters",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
