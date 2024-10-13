using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaintenanceService.Database.Migrations
{
    /// <inheritdoc />
    public partial class V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Maintenance",
                columns: new[] { "Id", "NextDailyDeliverySave" },
                values: new object[] { 1, new DateTime(2024, 10, 13, 0, 0, 0, 0, DateTimeKind.Local) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Maintenance",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
