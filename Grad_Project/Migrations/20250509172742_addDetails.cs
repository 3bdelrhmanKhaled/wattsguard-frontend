using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grad_Project.Migrations
{
    /// <inheritdoc />
    public partial class addDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_counterDatas_counters_counterId",
                table: "counterDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_counters_AspNetUsers_userId",
                table: "counters");

            migrationBuilder.DropForeignKey(
                name: "FK_counters_subAreas_subAreaId",
                table: "counters");

            migrationBuilder.DropForeignKey(
                name: "FK_subAreas_areas_areaId",
                table: "subAreas");

            migrationBuilder.DropForeignKey(
                name: "FK_userDevices_AspNetUsers_userId",
                table: "userDevices");

            migrationBuilder.DropForeignKey(
                name: "FK_userDevices_devices_deviceId",
                table: "userDevices");

            migrationBuilder.RenameColumn(
                name: "timeStamp",
                table: "counterDatas",
                newName: "TimeStamp");

            migrationBuilder.RenameColumn(
                name: "counterId",
                table: "counterDatas",
                newName: "CounterId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "counterDatas",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "usage",
                table: "counterDatas",
                newName: "Reading");

            migrationBuilder.RenameIndex(
                name: "IX_counterDatas_counterId",
                table: "counterDatas",
                newName: "IX_counterDatas_CounterId");

            migrationBuilder.AddColumn<int>(
                name: "Flag",
                table: "counterDatas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_counterDatas_counters_CounterId",
                table: "counterDatas",
                column: "CounterId",
                principalTable: "counters",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_counters_AspNetUsers_userId",
                table: "counters",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_counters_subAreas_subAreaId",
                table: "counters",
                column: "subAreaId",
                principalTable: "subAreas",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_subAreas_areas_areaId",
                table: "subAreas",
                column: "areaId",
                principalTable: "areas",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_userDevices_AspNetUsers_userId",
                table: "userDevices",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_userDevices_devices_deviceId",
                table: "userDevices",
                column: "deviceId",
                principalTable: "devices",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_counterDatas_counters_CounterId",
                table: "counterDatas");

            migrationBuilder.DropForeignKey(
                name: "FK_counters_AspNetUsers_userId",
                table: "counters");

            migrationBuilder.DropForeignKey(
                name: "FK_counters_subAreas_subAreaId",
                table: "counters");

            migrationBuilder.DropForeignKey(
                name: "FK_subAreas_areas_areaId",
                table: "subAreas");

            migrationBuilder.DropForeignKey(
                name: "FK_userDevices_AspNetUsers_userId",
                table: "userDevices");

            migrationBuilder.DropForeignKey(
                name: "FK_userDevices_devices_deviceId",
                table: "userDevices");

            migrationBuilder.DropColumn(
                name: "Flag",
                table: "counterDatas");

            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "counterDatas",
                newName: "timeStamp");

            migrationBuilder.RenameColumn(
                name: "CounterId",
                table: "counterDatas",
                newName: "counterId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "counterDatas",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Reading",
                table: "counterDatas",
                newName: "usage");

            migrationBuilder.RenameIndex(
                name: "IX_counterDatas_CounterId",
                table: "counterDatas",
                newName: "IX_counterDatas_counterId");

            migrationBuilder.AddForeignKey(
                name: "FK_counterDatas_counters_counterId",
                table: "counterDatas",
                column: "counterId",
                principalTable: "counters",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_counters_AspNetUsers_userId",
                table: "counters",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_counters_subAreas_subAreaId",
                table: "counters",
                column: "subAreaId",
                principalTable: "subAreas",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_subAreas_areas_areaId",
                table: "subAreas",
                column: "areaId",
                principalTable: "areas",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_userDevices_AspNetUsers_userId",
                table: "userDevices",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_userDevices_devices_deviceId",
                table: "userDevices",
                column: "deviceId",
                principalTable: "devices",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
