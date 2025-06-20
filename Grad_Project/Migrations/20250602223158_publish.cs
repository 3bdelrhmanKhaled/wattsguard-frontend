using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grad_Project.Migrations
{
    /// <inheritdoc />
    public partial class publish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AspNetUsers_UserId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_CounterData_counters_CounterId",
                table: "CounterData");

            migrationBuilder.DropForeignKey(
                name: "FK_OfficialReadings_counters_CounterId",
                table: "OfficialReadings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfficialReadings",
                table: "OfficialReadings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CounterData",
                table: "CounterData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses");

            migrationBuilder.RenameTable(
                name: "OfficialReadings",
                newName: "officialReadings");

            migrationBuilder.RenameTable(
                name: "CounterData",
                newName: "counterData");

            migrationBuilder.RenameTable(
                name: "Addresses",
                newName: "addresses");

            migrationBuilder.RenameIndex(
                name: "IX_OfficialReadings_CounterId",
                table: "officialReadings",
                newName: "IX_officialReadings_CounterId");

            migrationBuilder.RenameIndex(
                name: "IX_CounterData_CounterId",
                table: "counterData",
                newName: "IX_counterData_CounterId");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_UserId",
                table: "addresses",
                newName: "IX_addresses_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_officialReadings",
                table: "officialReadings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_counterData",
                table: "counterData",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_addresses",
                table: "addresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_addresses_AspNetUsers_UserId",
                table: "addresses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_counterData_counters_CounterId",
                table: "counterData",
                column: "CounterId",
                principalTable: "counters",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_officialReadings_counters_CounterId",
                table: "officialReadings",
                column: "CounterId",
                principalTable: "counters",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_addresses_AspNetUsers_UserId",
                table: "addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_counterData_counters_CounterId",
                table: "counterData");

            migrationBuilder.DropForeignKey(
                name: "FK_officialReadings_counters_CounterId",
                table: "officialReadings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_officialReadings",
                table: "officialReadings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_counterData",
                table: "counterData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_addresses",
                table: "addresses");

            migrationBuilder.RenameTable(
                name: "officialReadings",
                newName: "OfficialReadings");

            migrationBuilder.RenameTable(
                name: "counterData",
                newName: "CounterData");

            migrationBuilder.RenameTable(
                name: "addresses",
                newName: "Addresses");

            migrationBuilder.RenameIndex(
                name: "IX_officialReadings_CounterId",
                table: "OfficialReadings",
                newName: "IX_OfficialReadings_CounterId");

            migrationBuilder.RenameIndex(
                name: "IX_counterData_CounterId",
                table: "CounterData",
                newName: "IX_CounterData_CounterId");

            migrationBuilder.RenameIndex(
                name: "IX_addresses_UserId",
                table: "Addresses",
                newName: "IX_Addresses_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfficialReadings",
                table: "OfficialReadings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CounterData",
                table: "CounterData",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AspNetUsers_UserId",
                table: "Addresses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CounterData_counters_CounterId",
                table: "CounterData",
                column: "CounterId",
                principalTable: "counters",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfficialReadings_counters_CounterId",
                table: "OfficialReadings",
                column: "CounterId",
                principalTable: "counters",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
