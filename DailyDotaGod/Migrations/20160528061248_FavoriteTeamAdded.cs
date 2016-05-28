using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace DailyDotaGod.Migrations
{
    public partial class FavoriteTeamAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavoriteTeam",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Points = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteTeam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteTeam_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("FavoriteTeam");
        }
    }
}
