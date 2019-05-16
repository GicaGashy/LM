using Microsoft.EntityFrameworkCore.Migrations;

namespace LM.Data.Migrations
{
    public partial class BoolsUIM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInternetFacing",
                table: "Softwares",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMobile",
                table: "Softwares",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsUsed",
                table: "Softwares",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInternetFacing",
                table: "Softwares");

            migrationBuilder.DropColumn(
                name: "IsMobile",
                table: "Softwares");

            migrationBuilder.DropColumn(
                name: "IsUsed",
                table: "Softwares");
        }
    }
}
