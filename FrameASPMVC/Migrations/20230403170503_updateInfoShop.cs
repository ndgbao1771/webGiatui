using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FrameASPMVC.Migrations
{
    public partial class updateInfoShop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShopEmail",
                table: "InfoShop",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShopEmail",
                table: "InfoShop");
        }
    }
}
