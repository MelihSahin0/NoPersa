using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MaintenanceService.Database.Migrations
{
    /// <inheritdoc />
    public partial class V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FoodWish",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsIngredient = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodWish", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomersFoodWish",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    FoodWishId = table.Column<int>(type: "integer", nullable: false),
                    FoodWhishId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomersFoodWish", x => new { x.CustomerId, x.FoodWishId });
                    table.ForeignKey(
                        name: "FK_CustomersFoodWish_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomersFoodWish_FoodWish_FoodWishId",
                        column: x => x.FoodWishId,
                        principalTable: "FoodWish",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomersFoodWish_FoodWishId",
                table: "CustomersFoodWish",
                column: "FoodWishId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomersFoodWish");

            migrationBuilder.DropTable(
                name: "FoodWish");
        }
    }
}
