using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LinkExtractor.DAL.Migrations
{
    public partial class WorkshiftAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Workshifts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workshifts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeWorkshift",
                columns: table => new
                {
                    EmployeesId = table.Column<int>(type: "int", nullable: false),
                    WorkshiftsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeWorkshift", x => new { x.EmployeesId, x.WorkshiftsId });
                    table.ForeignKey(
                        name: "FK_EmployeeWorkshift_Employees_EmployeesId",
                        column: x => x.EmployeesId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeWorkshift_Workshifts_WorkshiftsId",
                        column: x => x.WorkshiftsId,
                        principalTable: "Workshifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWorkshift_WorkshiftsId",
                table: "EmployeeWorkshift",
                column: "WorkshiftsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeWorkshift");

            migrationBuilder.DropTable(
                name: "Workshifts");
        }
    }
}
