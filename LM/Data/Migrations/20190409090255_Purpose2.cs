using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LM.Data.Migrations
{
    public partial class Purpose2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PurposeId",
                table: "Softwares",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Purposes",
                columns: table => new
                {
                    PurposeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purposes", x => x.PurposeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Softwares_PurposeId",
                table: "Softwares",
                column: "PurposeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Softwares_Purposes_PurposeId",
                table: "Softwares",
                column: "PurposeId",
                principalTable: "Purposes",
                principalColumn: "PurposeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Softwares_Purposes_PurposeId",
                table: "Softwares");

            migrationBuilder.DropTable(
                name: "Purposes");

            migrationBuilder.DropIndex(
                name: "IX_Softwares_PurposeId",
                table: "Softwares");

            migrationBuilder.DropColumn(
                name: "PurposeId",
                table: "Softwares");
        }
    }
}
