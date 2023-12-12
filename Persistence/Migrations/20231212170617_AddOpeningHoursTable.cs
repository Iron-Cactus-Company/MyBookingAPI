using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOpeningHoursTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OpeningHours",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MondayStart = table.Column<int>(type: "integer", nullable: false),
                    MondayEnd = table.Column<int>(type: "integer", nullable: false),
                    TuesdayStart = table.Column<int>(type: "integer", nullable: false),
                    TuesdayEnd = table.Column<int>(type: "integer", nullable: false),
                    WednesdayStart = table.Column<int>(type: "integer", nullable: false),
                    WednesdayEnd = table.Column<int>(type: "integer", nullable: false),
                    ThursdayStart = table.Column<int>(type: "integer", nullable: false),
                    ThursdayEnd = table.Column<int>(type: "integer", nullable: false),
                    FridayStart = table.Column<int>(type: "integer", nullable: false),
                    FridayEnd = table.Column<int>(type: "integer", nullable: false),
                    SaturdayStart = table.Column<int>(type: "integer", nullable: false),
                    SaturdayEnd = table.Column<int>(type: "integer", nullable: false),
                    SundayStart = table.Column<int>(type: "integer", nullable: false),
                    SundayEnd = table.Column<int>(type: "integer", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpeningHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpeningHours_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OpeningHours_CompanyId",
                table: "OpeningHours",
                column: "CompanyId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OpeningHours");
        }
    }
}
