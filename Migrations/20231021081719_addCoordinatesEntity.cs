using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventSquareAPI.Migrations
{
    /// <inheritdoc />
    public partial class addCoordinatesEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location_Longitude",
                table: "Events",
                newName: "Location_Coordinates_Longitude");

            migrationBuilder.RenameColumn(
                name: "Location_Latitude",
                table: "Events",
                newName: "Location_Coordinates_Latitude");

            migrationBuilder.AddColumn<string>(
                name: "Location_Name",
                table: "Events",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Location_Postcode",
                table: "Events",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location_Name",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Location_Postcode",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "Location_Coordinates_Longitude",
                table: "Events",
                newName: "Location_Longitude");

            migrationBuilder.RenameColumn(
                name: "Location_Coordinates_Latitude",
                table: "Events",
                newName: "Location_Latitude");
        }
    }
}
