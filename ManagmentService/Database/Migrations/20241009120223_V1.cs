using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManagmentService.Database.Migrations
{
    /// <inheritdoc />
    public partial class V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TemporaryDelivery",
                table: "Customer",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "TemporaryNoDelivery",
                table: "Customer",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemporaryDelivery",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "TemporaryNoDelivery",
                table: "Customer");
        }
    }
}
