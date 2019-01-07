using Microsoft.EntityFrameworkCore.Migrations;

namespace GraniteHouse.Data.Migrations
{
    public partial class addSalesPersonToAppointmentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SalesPersonID",
                table: "appointments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_appointments_SalesPersonID",
                table: "appointments",
                column: "SalesPersonID");

            migrationBuilder.AddForeignKey(
                name: "FK_appointments_AspNetUsers_SalesPersonID",
                table: "appointments",
                column: "SalesPersonID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_appointments_AspNetUsers_SalesPersonID",
                table: "appointments");

            migrationBuilder.DropIndex(
                name: "IX_appointments_SalesPersonID",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "SalesPersonID",
                table: "appointments");
        }
    }
}
