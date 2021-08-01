using Microsoft.EntityFrameworkCore.Migrations;

namespace LinkExtractor.DAL.Migrations
{
    public partial class AddedEmployeeWorkshifts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeWorkshift");

            migrationBuilder.CreateTable(
                name: "EmployeeWorkshifts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    WorkshiftId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeWorkshifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeWorkshifts_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeWorkshifts_Workshifts_WorkshiftId",
                        column: x => x.WorkshiftId,
                        principalTable: "Workshifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWorkshifts_EmployeeId",
                table: "EmployeeWorkshifts",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWorkshifts_WorkshiftId",
                table: "EmployeeWorkshifts",
                column: "WorkshiftId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeWorkshifts");

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
    }
}
