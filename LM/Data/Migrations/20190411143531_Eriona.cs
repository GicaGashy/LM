using Microsoft.EntityFrameworkCore.Migrations;

namespace LM.Data.Migrations
{
    public partial class Eriona : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Softwares_Teams_BusinessOwnerTeamId",
                table: "Softwares");

            migrationBuilder.DropForeignKey(
                name: "FK_Softwares_Teams_ItOwnerTeamId",
                table: "Softwares");

            migrationBuilder.AlterColumn<int>(
                name: "ItOwnerTeamId",
                table: "Softwares",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "BusinessOwnerTeamId",
                table: "Softwares",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Softwares_Teams_BusinessOwnerTeamId",
                table: "Softwares",
                column: "BusinessOwnerTeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Softwares_Teams_ItOwnerTeamId",
                table: "Softwares",
                column: "ItOwnerTeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Softwares_Teams_BusinessOwnerTeamId",
                table: "Softwares");

            migrationBuilder.DropForeignKey(
                name: "FK_Softwares_Teams_ItOwnerTeamId",
                table: "Softwares");

            migrationBuilder.AlterColumn<int>(
                name: "ItOwnerTeamId",
                table: "Softwares",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BusinessOwnerTeamId",
                table: "Softwares",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Softwares_Teams_BusinessOwnerTeamId",
                table: "Softwares",
                column: "BusinessOwnerTeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Softwares_Teams_ItOwnerTeamId",
                table: "Softwares",
                column: "ItOwnerTeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
