using Microsoft.EntityFrameworkCore.Migrations;

namespace LM.Data.Migrations
{
    public partial class ChangedProduceToVendor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Producer",
                table: "Softwares",
                newName: "Vendor");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Vendor",
                table: "Softwares",
                newName: "Producer");
        }
    }
}
