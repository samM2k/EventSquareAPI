using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventSquareAPI.Migrations
{
    /// <inheritdoc />
    public partial class eventsAndRsvps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    StartDateTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    EndDateTime = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    IsVirtual = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsPhysical = table.Column<bool>(type: "INTEGER", nullable: false),
                    Location_FlatNumber = table.Column<int>(type: "INTEGER", nullable: true),
                    Location_StreetNumber = table.Column<int>(type: "INTEGER", nullable: true),
                    Location_StreetName = table.Column<string>(type: "TEXT", nullable: true),
                    Location_Locality = table.Column<string>(type: "TEXT", nullable: true),
                    Location_StateRegion = table.Column<string>(type: "TEXT", nullable: true),
                    Location_Country = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rsvps",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    EventId = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rsvps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rsvps_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rsvps_EventId",
                table: "Rsvps",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rsvps");

            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
