using Microsoft.EntityFrameworkCore.Migrations;

namespace Project2.Migrations
{
    public partial class Nova5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Recommendation",
                table: "Appointment");

            migrationBuilder.AddColumn<string>(
                name: "MedicalReport",
                table: "Appointment",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MedicalReport",
                table: "Appointment");

            migrationBuilder.AddColumn<string>(
                name: "Recommendation",
                table: "Appointment",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
