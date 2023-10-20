using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventSquareAPI.Migrations
{
    /// <inheritdoc />
    public partial class addLatLong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Location_Latitude",
                table: "Events",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Location_Longitude",
                table: "Events",
                type: "REAL",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location_Latitude",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Location_Longitude",
                table: "Events");
        }
    }
}
