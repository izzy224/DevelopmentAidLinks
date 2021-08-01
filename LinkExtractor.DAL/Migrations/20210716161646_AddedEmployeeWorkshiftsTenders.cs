using Microsoft.EntityFrameworkCore.Migrations;

namespace LinkExtractor.DAL.Migrations
{
    public partial class AddedEmployeeWorkshiftsTenders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tenders_Employees_EmployeeId",
                table: "Tenders");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenders_Workshifts_WorkshiftId",
                table: "Tenders");

            migrationBuilder.DropTable(
                name: "EmployeeWorkshift");

            migrationBuilder.DropIndex(
                name: "IX_Tenders_EmployeeId",
                table: "Tenders");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Tenders");

            migrationBuilder.RenameColumn(
                name: "WorkshiftId",
                table: "Tenders",
                newName: "EmployeeWorkshiftId");

            migrationBuilder.RenameIndex(
                name: "IX_Tenders_WorkshiftId",
                table: "Tenders",
                newName: "IX_Tenders_EmployeeWorkshiftId");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Tenders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Tenders_EmployeeWorkshifts_EmployeeWorkshiftId",
                table: "Tenders",
                column: "EmployeeWorkshiftId",
                principalTable: "EmployeeWorkshifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tenders_EmployeeWorkshifts_EmployeeWorkshiftId",
                table: "Tenders");

            migrationBuilder.DropTable(
                name: "EmployeeWorkshifts");

            migrationBuilder.RenameColumn(
                name: "EmployeeWorkshiftId",
                table: "Tenders",
                newName: "WorkshiftId");

            migrationBuilder.RenameIndex(
                name: "IX_Tenders_EmployeeWorkshiftId",
                table: "Tenders",
                newName: "IX_Tenders_WorkshiftId");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Tenders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Tenders",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                name: "IX_Tenders_EmployeeId",
                table: "Tenders",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWorkshift_WorkshiftsId",
                table: "EmployeeWorkshift",
                column: "WorkshiftsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tenders_Employees_EmployeeId",
                table: "Tenders",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tenders_Workshifts_WorkshiftId",
                table: "Tenders",
                column: "WorkshiftId",
                principalTable: "Workshifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
