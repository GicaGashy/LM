using Microsoft.EntityFrameworkCore.Migrations;

namespace LM.Data.Migrations
{
    public partial class TipiesAndTeachAreaModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tipies",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Tipies",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TechAreas",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TechAreas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "TechAreas",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Tipies");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "TechAreas");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "TechAreas");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Tipies",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TechAreas",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
