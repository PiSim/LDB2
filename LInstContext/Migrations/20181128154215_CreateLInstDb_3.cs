using Microsoft.EntityFrameworkCore.Migrations;

namespace LInst.Migrations
{
    public partial class CreateLInstDb_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "IsUnderControl",
                table: "Instruments",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<short>(
                name: "IsInService",
                table: "Instruments",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(short));

            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "InstrumentProperties",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<short>(
                name: "IsCalibrationProperty",
                table: "InstrumentProperties",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(short));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "IsUnderControl",
                table: "Instruments",
                nullable: false,
                oldClrType: typeof(short),
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<short>(
                name: "IsInService",
                table: "Instruments",
                nullable: false,
                oldClrType: typeof(short),
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "InstrumentProperties",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<short>(
                name: "IsCalibrationProperty",
                table: "InstrumentProperties",
                nullable: false,
                oldClrType: typeof(short),
                oldDefaultValue: false);
        }
    }
}
