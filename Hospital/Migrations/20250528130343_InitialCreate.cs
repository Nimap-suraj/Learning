using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hospital_OPD.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DailyAppointments",
                table: "DailyAppointments");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "DailyAppointments");

            migrationBuilder.RenameTable(
                name: "DailyAppointments",
                newName: "DailyAppointmentReportDto");

            migrationBuilder.AlterColumn<int>(
                name: "AppointmentCount",
                table: "DailyAppointmentReportDto",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "DailyAppointmentReportDto",
                newName: "DailyAppointments");

            migrationBuilder.AlterColumn<string>(
                name: "AppointmentCount",
                table: "DailyAppointments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "DailyAppointments",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DailyAppointments",
                table: "DailyAppointments",
                column: "Id");
        }
    }
}
