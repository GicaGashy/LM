using Microsoft.EntityFrameworkCore.Migrations;

namespace LM.Data.Migrations
{
    public partial class Er : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BusinessOwnerTeamId",
                table: "Softwares",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItOwnerTeamId",
                table: "Softwares",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Softwares_BusinessOwnerTeamId",
                table: "Softwares",
                column: "BusinessOwnerTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Softwares_ItOwnerTeamId",
                table: "Softwares",
                column: "ItOwnerTeamId");

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

            migrationBuilder.DropIndex(
                name: "IX_Softwares_BusinessOwnerTeamId",
                table: "Softwares");

            migrationBuilder.DropIndex(
                name: "IX_Softwares_ItOwnerTeamId",
                table: "Softwares");

            migrationBuilder.DropColumn(
                name: "BusinessOwnerTeamId",
                table: "Softwares");

            migrationBuilder.DropColumn(
                name: "ItOwnerTeamId",
                table: "Softwares");
        }
    }
}
