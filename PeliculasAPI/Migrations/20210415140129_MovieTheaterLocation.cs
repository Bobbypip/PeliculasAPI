using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

namespace PeliculasAPI.Migrations
{
    public partial class MovieTheaterLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Point>(
                name: "Location",
                table: "MovieTheaters",
                type: "geography",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "MovieTheaters");
        }
    }
}
