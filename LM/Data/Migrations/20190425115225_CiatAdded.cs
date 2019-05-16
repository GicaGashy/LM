using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LM.Data.Migrations
{
    public partial class CiatAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    Confidentiality = table.Column<int>(nullable: false),
                    Integrity = table.Column<int>(nullable: false),
                    Availability = table.Column<int>(nullable: false),
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
