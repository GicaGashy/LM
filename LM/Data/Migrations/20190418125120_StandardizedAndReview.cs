using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LM.Data.Migrations
{
    public partial class StandardizedAndReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Review",
                table: "Softwares",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Standardized",
                table: "Softwares",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Review",
                table: "Softwares");

            migrationBuilder.DropColumn(
                name: "Standardized",
                table: "Softwares");
        }
    }
}
