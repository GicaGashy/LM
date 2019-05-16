using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LM.Data.Migrations
{
    public partial class CiatRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Softwares_Ciat_CiatId",
                table: "Softwares");

            migrationBuilder.DropTable(
                name: "Ciat");

            migrationBuilder.DropIndex(
                name: "IX_Softwares_CiatId",
                table: "Softwares");

            migrationBuilder.DropColumn(
                name: "CiatId",
                table: "Softwares");

            migrationBuilder.AddColumn<int>(
                name: "Availability",
                table: "Softwares",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Confidentiality",
                table: "Softwares",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Integrity",
                table: "Softwares",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Traceability",
                table: "Softwares",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Availability",
                table: "Softwares");

            migrationBuilder.DropColumn(
                name: "Confidentiality",
                table: "Softwares");

            migrationBuilder.DropColumn(
                name: "Integrity",
                table: "Softwares");

            migrationBuilder.DropColumn(
                name: "Traceability",
                table: "Softwares");

            migrationBuilder.AddColumn<int>(
                name: "CiatId",
                table: "Softwares",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Ciat",
                columns: table => new
                {
                    CiatId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Availability = table.Column<int>(nullable: false),
                    Confidentiality = table.Column<int>(nullable: false),
                    Integrity = table.Column<int>(nullable: false),
                    Traceability = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ciat", x => x.CiatId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Softwares_CiatId",
                table: "Softwares",
                column: "CiatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Softwares_Ciat_CiatId",
                table: "Softwares",
                column: "CiatId",
                principalTable: "Ciat",
                principalColumn: "CiatId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
