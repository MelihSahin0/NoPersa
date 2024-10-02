using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DeliveryService.Database.Migrations
{
    /// <inheritdoc />
    public partial class V0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyOverview",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Price = table.Column<double>(type: "double precision", nullable: true),
                    NumberOfBoxes = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyOverview", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Position = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Weekdays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Monday = table.Column<bool>(type: "boolean", nullable: false),
                    Tuesday = table.Column<bool>(type: "boolean", nullable: false),
                    Wednesday = table.Column<bool>(type: "boolean", nullable: false),
                    Thursday = table.Column<bool>(type: "boolean", nullable: false),
                    Friday = table.Column<bool>(type: "boolean", nullable: false),
                    Saturday = table.Column<bool>(type: "boolean", nullable: false),
                    Sunday = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weekdays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SerialNumber = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Address = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Region = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    GeoLocation = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    ContactInformation = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Article = table.Column<int>(type: "integer", nullable: false),
                    DefaultPrice = table.Column<double>(type: "double precision", nullable: false),
                    DefaultNumberOfBoxes = table.Column<int>(type: "integer", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: true),
                    WorkdaysId = table.Column<int>(type: "integer", nullable: false),
                    HolidaysId = table.Column<int>(type: "integer", nullable: false),
                    RouteId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Customer_Weekdays_HolidaysId",
                        column: x => x.HolidaysId,
                        principalTable: "Weekdays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Customer_Weekdays_WorkdaysId",
                        column: x => x.WorkdaysId,
                        principalTable: "Weekdays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyOverview",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Month = table.Column<int>(type: "integer", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Day1Id = table.Column<int>(type: "integer", nullable: false),
                    Day2Id = table.Column<int>(type: "integer", nullable: false),
                    Day3Id = table.Column<int>(type: "integer", nullable: false),
                    Day4Id = table.Column<int>(type: "integer", nullable: false),
                    Day5Id = table.Column<int>(type: "integer", nullable: false),
                    Day6Id = table.Column<int>(type: "integer", nullable: false),
                    Day7Id = table.Column<int>(type: "integer", nullable: false),
                    Day8Id = table.Column<int>(type: "integer", nullable: false),
                    Day9Id = table.Column<int>(type: "integer", nullable: false),
                    Day10Id = table.Column<int>(type: "integer", nullable: false),
                    Day11Id = table.Column<int>(type: "integer", nullable: false),
                    Day12Id = table.Column<int>(type: "integer", nullable: false),
                    Day13Id = table.Column<int>(type: "integer", nullable: false),
                    Day14Id = table.Column<int>(type: "integer", nullable: false),
                    Day15Id = table.Column<int>(type: "integer", nullable: false),
                    Day16Id = table.Column<int>(type: "integer", nullable: false),
                    Day17Id = table.Column<int>(type: "integer", nullable: false),
                    Day18Id = table.Column<int>(type: "integer", nullable: false),
                    Day19Id = table.Column<int>(type: "integer", nullable: false),
                    Day20Id = table.Column<int>(type: "integer", nullable: false),
                    Day21Id = table.Column<int>(type: "integer", nullable: false),
                    Day22Id = table.Column<int>(type: "integer", nullable: false),
                    Day23Id = table.Column<int>(type: "integer", nullable: false),
                    Day24Id = table.Column<int>(type: "integer", nullable: false),
                    Day25Id = table.Column<int>(type: "integer", nullable: false),
                    Day26Id = table.Column<int>(type: "integer", nullable: false),
                    Day27Id = table.Column<int>(type: "integer", nullable: false),
                    Day28Id = table.Column<int>(type: "integer", nullable: false),
                    Day29Id = table.Column<int>(type: "integer", nullable: false),
                    Day30Id = table.Column<int>(type: "integer", nullable: false),
                    Day31Id = table.Column<int>(type: "integer", nullable: false),
                    CustomerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyOverview", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_DailyOverview_Day10Id",
                        column: x => x.Day10Id,
                        principalTable: "DailyOverview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_DailyOverview_Day11Id",
                        column: x => x.Day11Id,
                        principalTable: "DailyOverview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_DailyOverview_Day12Id",
                        column: x => x.Day12Id,
                        principalTable: "DailyOverview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_DailyOverview_Day13Id",
                        column: x => x.Day13Id,
                        principalTable: "DailyOverview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_DailyOverview_Day14Id",
                        column: x => x.Day14Id,
                        principalTable: "DailyOverview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_DailyOverview_Day15Id",
                        column: x => x.Day15Id,
                        principalTable: "DailyOverview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_DailyOverview_Day16Id",
                        column: x => x.Day16Id,
                        principalTable: "DailyOverview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_DailyOverview_Day17Id",
                        column: x => x.Day17Id,
                        principalTable: "DailyOverview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_DailyOverview_Day18Id",
                        column: x => x.Day18Id,
                        principalTable: "DailyOverview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_DailyOverview_Day19Id",
                        column: x => x.Day19Id,
                        principalTable: "DailyOverview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_DailyOverview_Day1Id",
                        column: x => x.Day1Id,
                        principalTable: "DailyOverview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_DailyOverview_Day20Id",
                        column: x => x.Day20Id,
                        principalTable: "DailyOverview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_DailyOverview_Day21Id",
                        column: x => x.Day21Id,
                        principalTable: "DailyOverview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_DailyOverview_Day22Id",
                        column: x => x.Day22Id,
                        principalTable: "DailyOverview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_DailyOverview_Day23Id",
                        column: x => x.Day23Id,
                        principalTable: "DailyOverview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_DailyOverview_Day24Id",
                        column: x => x.Day24Id,
                        principalTable: "DailyOverview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_DailyOverview_Day25Id",
                        column: x => x.Day25Id,
                        principalTable: "DailyOverview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_DailyOverview_Day26Id",
                        column: x => x.Day26Id,
                        principalTable: "DailyOverview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_DailyOverview_Day27Id",
                        column: x => x.Day27Id,
                        principalTable: "DailyOverview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_DailyOverview_Day28Id",
                        column: x => x.Day28Id,
                        principalTable: "DailyOverview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_DailyOverview_Day29Id",
                        column: x => x.Day29Id,
                        principalTable: "DailyOverview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_DailyOverview_Day2Id",
                        column: x => x.Day2Id,
                        principalTable: "DailyOverview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_DailyOverview_Day30Id",
                        column: x => x.Day30Id,
                        principalTable: "DailyOverview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_DailyOverview_Day31Id",
                        column: x => x.Day31Id,
                        principalTable: "DailyOverview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_DailyOverview_Day3Id",
                        column: x => x.Day3Id,
                        principalTable: "DailyOverview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_DailyOverview_Day4Id",
                        column: x => x.Day4Id,
                        principalTable: "DailyOverview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_DailyOverview_Day5Id",
                        column: x => x.Day5Id,
                        principalTable: "DailyOverview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_DailyOverview_Day6Id",
                        column: x => x.Day6Id,
                        principalTable: "DailyOverview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_DailyOverview_Day7Id",
                        column: x => x.Day7Id,
                        principalTable: "DailyOverview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_DailyOverview_Day8Id",
                        column: x => x.Day8Id,
                        principalTable: "DailyOverview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonthlyOverview_DailyOverview_Day9Id",
                        column: x => x.Day9Id,
                        principalTable: "DailyOverview",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_HolidaysId",
                table: "Customer",
                column: "HolidaysId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_RouteId",
                table: "Customer",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_WorkdaysId",
                table: "Customer",
                column: "WorkdaysId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_CustomerId",
                table: "MonthlyOverview",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_Day10Id",
                table: "MonthlyOverview",
                column: "Day10Id");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_Day11Id",
                table: "MonthlyOverview",
                column: "Day11Id");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_Day12Id",
                table: "MonthlyOverview",
                column: "Day12Id");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_Day13Id",
                table: "MonthlyOverview",
                column: "Day13Id");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_Day14Id",
                table: "MonthlyOverview",
                column: "Day14Id");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_Day15Id",
                table: "MonthlyOverview",
                column: "Day15Id");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_Day16Id",
                table: "MonthlyOverview",
                column: "Day16Id");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_Day17Id",
                table: "MonthlyOverview",
                column: "Day17Id");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_Day18Id",
                table: "MonthlyOverview",
                column: "Day18Id");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_Day19Id",
                table: "MonthlyOverview",
                column: "Day19Id");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_Day1Id",
                table: "MonthlyOverview",
                column: "Day1Id");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_Day20Id",
                table: "MonthlyOverview",
                column: "Day20Id");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_Day21Id",
                table: "MonthlyOverview",
                column: "Day21Id");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_Day22Id",
                table: "MonthlyOverview",
                column: "Day22Id");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_Day23Id",
                table: "MonthlyOverview",
                column: "Day23Id");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_Day24Id",
                table: "MonthlyOverview",
                column: "Day24Id");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_Day25Id",
                table: "MonthlyOverview",
                column: "Day25Id");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_Day26Id",
                table: "MonthlyOverview",
                column: "Day26Id");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_Day27Id",
                table: "MonthlyOverview",
                column: "Day27Id");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_Day28Id",
                table: "MonthlyOverview",
                column: "Day28Id");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_Day29Id",
                table: "MonthlyOverview",
                column: "Day29Id");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_Day2Id",
                table: "MonthlyOverview",
                column: "Day2Id");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_Day30Id",
                table: "MonthlyOverview",
                column: "Day30Id");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_Day31Id",
                table: "MonthlyOverview",
                column: "Day31Id");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_Day3Id",
                table: "MonthlyOverview",
                column: "Day3Id");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_Day4Id",
                table: "MonthlyOverview",
                column: "Day4Id");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_Day5Id",
                table: "MonthlyOverview",
                column: "Day5Id");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_Day6Id",
                table: "MonthlyOverview",
                column: "Day6Id");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_Day7Id",
                table: "MonthlyOverview",
                column: "Day7Id");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_Day8Id",
                table: "MonthlyOverview",
                column: "Day8Id");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyOverview_Day9Id",
                table: "MonthlyOverview",
                column: "Day9Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonthlyOverview");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "DailyOverview");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "Weekdays");
        }
    }
}
