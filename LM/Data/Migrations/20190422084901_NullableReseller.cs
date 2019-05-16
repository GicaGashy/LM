using Microsoft.EntityFrameworkCore.Migrations;

namespace LM.Data.Migrations
{
    public partial class NullableReseller : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Softwares_Reseller_ResellerId",
                table: "Softwares");

            migrationBuilder.AlterColumn<int>(
                name: "ResellerId",
                table: "Softwares",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Softwares_Reseller_ResellerId",
                table: "Softwares",
                column: "ResellerId",
                principalTable: "Reseller",
                principalColumn: "ResellerId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Softwares_Reseller_ResellerId",
                table: "Softwares");

            migrationBuilder.AlterColumn<int>(
                name: "ResellerId",
                table: "Softwares",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Softwares_Reseller_ResellerId",
                table: "Softwares",
                column: "ResellerId",
                principalTable: "Reseller",
                principalColumn: "ResellerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
