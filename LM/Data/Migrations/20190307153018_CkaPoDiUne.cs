using Microsoft.EntityFrameworkCore.Migrations;

namespace LM.Data.Migrations
{
    public partial class CkaPoDiUne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SoftwareId",
                table: "Teams",
                nullable: true);


            migrationBuilder.CreateIndex(
                name: "IX_Teams_SoftwareId",
                table: "Teams",
                column: "SoftwareId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Softwares_SoftwareId",
                table: "Teams",
                column: "SoftwareId",
                principalTable: "Softwares",
                principalColumn: "SoftwareId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Softwares_SoftwareId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_SoftwareId",
                table: "Teams");

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "SoftwareId",
                table: "Teams");
        }
    }
}
