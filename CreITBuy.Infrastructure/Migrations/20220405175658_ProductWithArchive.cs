using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CreITBuy.Infrastructures.Migrations
{
    public partial class ProductWithArchive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ProductArchive",
                table: "Products",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductArchive",
                table: "Products");
        }
    }
}
