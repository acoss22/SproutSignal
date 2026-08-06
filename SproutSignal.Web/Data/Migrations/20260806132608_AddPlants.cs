using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SproutSignal.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPlants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Plants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Species = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    Location = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    WateringIntervalDays = table.Column<int>(type: "INTEGER", nullable: false),
                    LastWateredAtUtc = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    OwnerId = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plants", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Plants");
        }
    }
}
