using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaintenanceService.Database.Migrations
{
    /// <inheritdoc />
    public partial class V2 : Migration
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

            migrationBuilder.DropTable(
                name: "CustomerLightDiet");

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

            migrationBuilder.AddColumn<bool>(
                name: "Selected",
                table: "CustomersLightDiet",
                type: "boolean",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.DropColumn(
                name: "Selected",
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

            migrationBuilder.CreateTable(
                name: "CustomerLightDiet",
                columns: table => new
                {
                    CustomersId = table.Column<int>(type: "integer", nullable: false),
                    LightDietsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerLightDiet", x => new { x.CustomersId, x.LightDietsId });
                    table.ForeignKey(
                        name: "FK_CustomerLightDiet_Customer_CustomersId",
                        column: x => x.CustomersId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerLightDiet_LightDiet_LightDietsId",
                        column: x => x.LightDietsId,
                        principalTable: "LightDiet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerLightDiet_LightDietsId",
                table: "CustomerLightDiet",
                column: "LightDietsId");

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
