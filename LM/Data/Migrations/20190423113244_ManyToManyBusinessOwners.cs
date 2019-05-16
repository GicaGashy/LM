using Microsoft.EntityFrameworkCore.Migrations;

namespace LM.Data.Migrations
{
    public partial class ManyToManyBusinessOwners : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SoftwareBusinessOwnerTeams",
                columns: table => new
                {
                    SoftwareId = table.Column<int>(nullable: false),
                    BusinessOwnerTeamId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftwareBusinessOwnerTeams", x => new { x.SoftwareId, x.BusinessOwnerTeamId });
                    table.ForeignKey(
                        name: "FK_SoftwareBusinessOwnerTeams_Teams_BusinessOwnerTeamId",
                        column: x => x.BusinessOwnerTeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SoftwareBusinessOwnerTeams_Softwares_SoftwareId",
                        column: x => x.SoftwareId,
                        principalTable: "Softwares",
                        principalColumn: "SoftwareId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareBusinessOwnerTeams_BusinessOwnerTeamId",
                table: "SoftwareBusinessOwnerTeams",
                column: "BusinessOwnerTeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SoftwareBusinessOwnerTeams");
        }
    }
}
