using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Grad_Project.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropTable(
                name: "schedules");

            migrationBuilder.DropTable(
                name: "subAreas");

            migrationBuilder.DropTable(
                name: "userDevices");

            migrationBuilder.DropTable(
                name: "areas");

            migrationBuilder.DropTable(
                name: "devices");

            migrationBuilder.DropIndex(
                name: "IX_counters_subAreaId",
                table: "counters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_counterDatas",
                table: "counterDatas");

            migrationBuilder.DropColumn(
                name: "subAreaId",
                table: "counters");

            migrationBuilder.RenameTable(
                name: "counterDatas",
                newName: "CounterData");

            migrationBuilder.RenameIndex(
                name: "IX_counterDatas_CounterId",
                table: "CounterData",
                newName: "IX_CounterData_CounterId");

            migrationBuilder.AddColumn<string>(
                name: "CounterId",
                table: "counters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CounterData",
                table: "CounterData",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Governorate = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OfficialReadings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CounterId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Reading = table.Column<double>(type: "float", nullable: false),
                    ReadingDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficialReadings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfficialReadings_counters_CounterId",
                        column: x => x.CounterId,
                        principalTable: "counters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UserId",
                table: "Addresses",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfficialReadings_CounterId",
                table: "OfficialReadings",
                column: "CounterId");

            migrationBuilder.AddForeignKey(
                name: "FK_CounterData_counters_CounterId",
                table: "CounterData",
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
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CounterData_counters_CounterId",
                table: "CounterData");

            migrationBuilder.DropForeignKey(
                name: "FK_counters_AspNetUsers_userId",
                table: "counters");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "OfficialReadings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CounterData",
                table: "CounterData");

            migrationBuilder.DropColumn(
                name: "CounterId",
                table: "counters");

            migrationBuilder.RenameTable(
                name: "CounterData",
                newName: "counterDatas");

            migrationBuilder.RenameIndex(
                name: "IX_CounterData_CounterId",
                table: "counterDatas",
                newName: "IX_counterDatas_CounterId");

            migrationBuilder.AddColumn<int>(
                name: "subAreaId",
                table: "counters",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_counterDatas",
                table: "counterDatas",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "areas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    totalUsage = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_areas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "devices",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    powerConsumption = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_devices", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "subAreas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    areaId = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    usage = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subAreas", x => x.id);
                    table.ForeignKey(
                        name: "FK_subAreas_areas_areaId",
                        column: x => x.areaId,
                        principalTable: "areas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "schedules",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    deviceId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    endTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    startTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schedules", x => x.id);
                    table.ForeignKey(
                        name: "FK_schedules_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_schedules_devices_deviceId",
                        column: x => x.deviceId,
                        principalTable: "devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "userDevices",
                columns: table => new
                {
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    deviceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userDevices", x => new { x.userId, x.deviceId });
                    table.ForeignKey(
                        name: "FK_userDevices_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_userDevices_devices_deviceId",
                        column: x => x.deviceId,
                        principalTable: "devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_counters_subAreaId",
                table: "counters",
                column: "subAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_schedules_deviceId",
                table: "schedules",
                column: "deviceId");

            migrationBuilder.CreateIndex(
                name: "IX_schedules_userId",
                table: "schedules",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_subAreas_areaId",
                table: "subAreas",
                column: "areaId");

            migrationBuilder.CreateIndex(
                name: "IX_userDevices_deviceId",
                table: "userDevices",
                column: "deviceId");

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
        }
    }
}
