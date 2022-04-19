using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Migrations
{
    public partial class GameModify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSmallerMode",
                table: "Games",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSmallerMode",
                table: "Games");
        }
    }
}
