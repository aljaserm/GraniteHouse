using Microsoft.EntityFrameworkCore.Migrations;

namespace GraniteHouse.Data.Migrations
{
    public partial class madeNameRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "specialTags",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "specialTags",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
