using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortener.Migrations
{
    public partial class Add_Zaply_Schema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "zaply");

            migrationBuilder.RenameTable(
                name: "shortened_urls",
                newName: "shortened_urls",
                newSchema: "zaply");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "shortened_urls",
                schema: "zaply",
                newName: "shortened_urls");
        }
    }
}
