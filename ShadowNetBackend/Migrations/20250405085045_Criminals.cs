using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShadowNetBackend.Migrations
{
    /// <inheritdoc />
    public partial class Criminals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Criminals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    LastName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Alias = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    DateOfBirth = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Nationality = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    KnownAffiliations = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    ThreatLevel = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsArmedAndDangerous = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    LastKnownLocation = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    LastSpottedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UnderSurveillance = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    SurveillanceNotes = table.Column<string>(type: "text", nullable: true),
                    Image = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Criminals", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Criminals");
        }
    }
}
