using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShadowNetBackend.Migrations
{
    /// <inheritdoc />
    public partial class OneToManyAgentMissionRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MissionAssignments");

            migrationBuilder.AddColumn<Guid>(
                name: "MissionId",
                table: "Agents",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agents_MissionId",
                table: "Agents",
                column: "MissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agents_Missions_MissionId",
                table: "Agents",
                column: "MissionId",
                principalTable: "Missions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agents_Missions_MissionId",
                table: "Agents");

            migrationBuilder.DropIndex(
                name: "IX_Agents_MissionId",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "MissionId",
                table: "Agents");

            migrationBuilder.CreateTable(
                name: "MissionAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AgentId = table.Column<string>(type: "text", nullable: false),
                    MissionId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssignedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MissionAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MissionAssignments_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MissionAssignments_Missions_MissionId",
                        column: x => x.MissionId,
                        principalTable: "Missions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MissionAssignments_AgentId_MissionId",
                table: "MissionAssignments",
                columns: new[] { "AgentId", "MissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MissionAssignments_MissionId",
                table: "MissionAssignments",
                column: "MissionId");
        }
    }
}
