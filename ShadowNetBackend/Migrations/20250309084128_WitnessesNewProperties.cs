using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShadowNetBackend.Migrations
{
    /// <inheritdoc />
    public partial class WitnessesNewProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Witnesses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Witnesses",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Witnesses",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Witnesses");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Witnesses");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Witnesses");
        }
    }
}
