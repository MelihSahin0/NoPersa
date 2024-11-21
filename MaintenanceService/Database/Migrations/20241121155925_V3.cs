using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MaintenanceService.Database.Migrations
{
    /// <inheritdoc />
    public partial class V3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultPrice",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "NextDailyDeliverySave",
                table: "Maintenance",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "Article",
                table: "Customer",
                newName: "ArticleId");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Maintenance",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Article",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Position = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    NewPrice = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_ArticleId",
                table: "Customer",
                column: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Article_ArticleId",
                table: "Customer",
                column: "ArticleId",
                principalTable: "Article",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Article_ArticleId",
                table: "Customer");

            migrationBuilder.DropTable(
                name: "Article");

            migrationBuilder.DropIndex(
                name: "IX_Customer_ArticleId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Maintenance");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Maintenance",
                newName: "NextDailyDeliverySave");

            migrationBuilder.RenameColumn(
                name: "ArticleId",
                table: "Customer",
                newName: "Article");

            migrationBuilder.AddColumn<double>(
                name: "DefaultPrice",
                table: "Customer",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
