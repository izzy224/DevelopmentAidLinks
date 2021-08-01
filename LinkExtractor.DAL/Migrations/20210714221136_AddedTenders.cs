using Microsoft.EntityFrameworkCore.Migrations;

namespace LinkExtractor.DAL.Migrations
{
    public partial class AddedTenders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tenders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkshiftId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tenders_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tenders_Workshifts_WorkshiftId",
                        column: x => x.WorkshiftId,
                        principalTable: "Workshifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tenders_EmployeeId",
                table: "Tenders",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenders_WorkshiftId",
                table: "Tenders",
                column: "WorkshiftId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tenders");
        }
    }
}
