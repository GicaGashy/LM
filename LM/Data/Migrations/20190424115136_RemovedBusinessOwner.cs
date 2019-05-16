using Microsoft.EntityFrameworkCore.Migrations;

namespace LM.Data.Migrations
{
    public partial class RemovedBusinessOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Softwares_Teams_BusinessOwnerTeamId",
                table: "Softwares");

            migrationBuilder.DropIndex(
                name: "IX_Softwares_BusinessOwnerTeamId",
                table: "Softwares");

            migrationBuilder.DropColumn(
                name: "BusinessOwnerTeamId",
                table: "Softwares");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BusinessOwnerTeamId",
                table: "Softwares",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Softwares_BusinessOwnerTeamId",
                table: "Softwares",
                column: "BusinessOwnerTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Softwares_Teams_BusinessOwnerTeamId",
                table: "Softwares",
                column: "BusinessOwnerTeamId",
                principalTable: "Teams",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
