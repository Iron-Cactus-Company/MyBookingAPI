using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessProfile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    AcceptingTimeText = table.Column<string>(type: "text", nullable: true),
                    CancellingTimeText = table.Column<string>(type: "text", nullable: true),
                    BusinessProfileId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Company_BusinessProfile_BusinessProfileId",
                        column: x => x.BusinessProfileId,
                        principalTable: "BusinessProfile",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Duration = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Service_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExceptionHours",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Start = table.Column<int>(type: "integer", nullable: false),
                    End = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    OpeningHoursId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExceptionHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExceptionHours_OpeningHours_OpeningHoursId",
                        column: x => x.OpeningHoursId,
                        principalTable: "OpeningHours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Company_BusinessProfileId",
                table: "Company",
                column: "BusinessProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionHours_OpeningHoursId",
                table: "ExceptionHours",
                column: "OpeningHoursId");

            migrationBuilder.CreateIndex(
                name: "IX_OpeningHours_CompanyId",
                table: "OpeningHours",
                column: "CompanyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Service_CompanyId",
                table: "Service",
                column: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExceptionHours");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "OpeningHours");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "BusinessProfile");
        }
    }
}
