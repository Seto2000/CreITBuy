using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CreITBuy.Infrastructures.Migrations
{
    public partial class AddingLiveInToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardType",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LiveIn",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardType",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "LiveIn",
                table: "AspNetUsers");
        }
    }
}
