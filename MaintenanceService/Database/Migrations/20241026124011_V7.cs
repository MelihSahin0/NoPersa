using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaintenanceService.Database.Migrations
{
    /// <inheritdoc />
    public partial class V7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomersLightDiet_Customer_CustomerId",
                table: "CustomersLightDiet");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomersLightDiet_LightDiet_LightDietId",
                table: "CustomersLightDiet");

            migrationBuilder.RenameColumn(
                name: "LightDietId",
                table: "CustomersLightDiet",
                newName: "LightDietsId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "CustomersLightDiet",
                newName: "CustomersId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomersLightDiet_LightDietId",
                table: "CustomersLightDiet",
                newName: "IX_CustomersLightDiet_LightDietsId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomersLightDiet_Customer_CustomersId",
                table: "CustomersLightDiet",
                column: "CustomersId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomersLightDiet_LightDiet_LightDietsId",
                table: "CustomersLightDiet",
                column: "LightDietsId",
                principalTable: "LightDiet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomersLightDiet_Customer_CustomersId",
                table: "CustomersLightDiet");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomersLightDiet_LightDiet_LightDietsId",
                table: "CustomersLightDiet");

            migrationBuilder.RenameColumn(
                name: "LightDietsId",
                table: "CustomersLightDiet",
                newName: "LightDietId");

            migrationBuilder.RenameColumn(
                name: "CustomersId",
                table: "CustomersLightDiet",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_CustomersLightDiet_LightDietsId",
                table: "CustomersLightDiet",
                newName: "IX_CustomersLightDiet_LightDietId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomersLightDiet_Customer_CustomerId",
                table: "CustomersLightDiet",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomersLightDiet_LightDiet_LightDietId",
                table: "CustomersLightDiet",
                column: "LightDietId",
                principalTable: "LightDiet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
