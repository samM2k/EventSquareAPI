using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventSquareAPI.Migrations
{
    /// <inheritdoc />
    public partial class eventVisibilityAndOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "Events",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Visibility",
                table: "Events",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Visibility",
                table: "Events");
        }
    }
}
