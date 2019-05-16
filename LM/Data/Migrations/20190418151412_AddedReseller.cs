using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LM.Data.Migrations
{
    public partial class AddedReseller : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResellerId",
                table: "Softwares",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Reseller",
                columns: table => new
                {
                    ResellerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Country = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reseller", x => x.ResellerId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Softwares_ResellerId",
                table: "Softwares",
                column: "ResellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Softwares_Reseller_ResellerId",
                table: "Softwares",
                column: "ResellerId",
                principalTable: "Reseller",
                principalColumn: "ResellerId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Softwares_Reseller_ResellerId",
                table: "Softwares");

            migrationBuilder.DropTable(
                name: "Reseller");

            migrationBuilder.DropIndex(
                name: "IX_Softwares_ResellerId",
                table: "Softwares");

            migrationBuilder.DropColumn(
                name: "ResellerId",
                table: "Softwares");
        }
    }
}
