using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShadowNetBackend.Migrations
{
    /// <inheritdoc />
    public partial class MissionAndAgentNewProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "Missions",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Missions",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Missions",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Agents",
                type: "bytea",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Missions");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Missions");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Missions");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Agents");
        }
    }
}
