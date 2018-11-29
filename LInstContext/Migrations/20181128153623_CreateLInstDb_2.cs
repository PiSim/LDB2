using Microsoft.EntityFrameworkCore.Migrations;

namespace LInst.Migrations
{
    public partial class CreateLInstDb_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "InstrumentProperties",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "InstrumentProperties",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
