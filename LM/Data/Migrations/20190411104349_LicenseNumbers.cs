using Microsoft.EntityFrameworkCore.Migrations;

namespace LM.Data.Migrations
{
    public partial class LicenseNumbers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LicensesInUse",
                table: "Softwares",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalLicenses",
                table: "Softwares",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LicensesInUse",
                table: "Softwares");

            migrationBuilder.DropColumn(
                name: "TotalLicenses",
                table: "Softwares");
        }
    }
}
