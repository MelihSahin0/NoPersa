using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MaintenanceService.Database.Migrations
{
    /// <inheritdoc />
    public partial class V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Route",
                keyColumn: "Id",
                keyValue: -2147483648);

            migrationBuilder.CreateTable(
                name: "LightDiet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LightDiet", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "CustomersLightDiet",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    LightDietId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomersLightDiet", x => new { x.CustomerId, x.LightDietId });
                    table.ForeignKey(
                        name: "FK_CustomersLightDiet_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomersLightDiet_LightDiet_LightDietId",
                        column: x => x.LightDietId,
                        principalTable: "LightDiet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerLightDiet_LightDietsId",
                table: "CustomerLightDiet",
                column: "LightDietsId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomersLightDiet_LightDietId",
                table: "CustomersLightDiet",
                column: "LightDietId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerLightDiet");

            migrationBuilder.DropTable(
                name: "CustomersLightDiet");

            migrationBuilder.DropTable(
                name: "LightDiet");

            migrationBuilder.InsertData(
                table: "Route",
                columns: new[] { "Id", "Name", "Position" },
                values: new object[] { -2147483648, "Archive", 2147483647 });
        }
    }
}
