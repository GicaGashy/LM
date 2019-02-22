using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LM.Data.Migrations
{
    public partial class AlmostDone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TechAreas",
                columns: table => new
                {
                    TechAreaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechAreas", x => x.TechAreaId);
                });

            migrationBuilder.CreateTable(
                name: "Tipies",
                columns: table => new
                {
                    TipiId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Rate = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipies", x => x.TipiId);
                });

            migrationBuilder.CreateTable(
                name: "Softwares",
                columns: table => new
                {
                    SoftwareId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true),
                    LicenseStart = table.Column<DateTime>(nullable: false),
                    LicenseEnd = table.Column<DateTime>(nullable: false),
                    UseCases = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    TechAreaId = table.Column<int>(nullable: false),
                    TipiId = table.Column<int>(nullable: false),
                    AppUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Softwares", x => x.SoftwareId);
                    table.ForeignKey(
                        name: "FK_Softwares_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Softwares_TechAreas_TechAreaId",
                        column: x => x.TechAreaId,
                        principalTable: "TechAreas",
                        principalColumn: "TechAreaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Softwares_Tipies_TipiId",
                        column: x => x.TipiId,
                        principalTable: "Tipies",
                        principalColumn: "TipiId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SoftwareTeams",
                columns: table => new
                {
                    SoftwareId = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftwareTeams", x => new { x.SoftwareId, x.TeamId });
                    table.ForeignKey(
                        name: "FK_SoftwareTeams_Softwares_SoftwareId",
                        column: x => x.SoftwareId,
                        principalTable: "Softwares",
                        principalColumn: "SoftwareId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SoftwareTeams_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Softwares_AppUserId",
                table: "Softwares",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Softwares_TechAreaId",
                table: "Softwares",
                column: "TechAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Softwares_TipiId",
                table: "Softwares",
                column: "TipiId");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareTeams_TeamId",
                table: "SoftwareTeams",
                column: "TeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SoftwareTeams");

            migrationBuilder.DropTable(
                name: "Softwares");

            migrationBuilder.DropTable(
                name: "TechAreas");

            migrationBuilder.DropTable(
                name: "Tipies");
        }
    }
}
