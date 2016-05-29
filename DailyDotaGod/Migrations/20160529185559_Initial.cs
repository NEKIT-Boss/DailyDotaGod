using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace DailyDotaGod.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CountryImage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(nullable: true),
                    Data = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryImage", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "LeagueImage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Data = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueImage", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "TeamImage",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Data = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamImage", x => x.Id);
                });
            migrationBuilder.CreateTable(
                name: "League",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LogoId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_League", x => x.Id);
                    table.ForeignKey(
                        name: "FK_League_LeagueImage_LogoId",
                        column: x => x.LogoId,
                        principalTable: "LeagueImage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CountryLogoId = table.Column<int>(nullable: true),
                    LogoId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Tag = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Team_CountryImage_CountryLogoId",
                        column: x => x.CountryLogoId,
                        principalTable: "CountryImage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Team_TeamImage_LogoId",
                        column: x => x.LogoId,
                        principalTable: "TeamImage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
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
            migrationBuilder.CreateTable(
                name: "Match",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BestOf = table.Column<int>(nullable: false),
                    LeagueId = table.Column<int>(nullable: true),
                    LiveStatus = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    Team1Id = table.Column<int>(nullable: true),
                    Team2Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Match", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Match_League_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "League",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Match_Team_Team1Id",
                        column: x => x.Team1Id,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Match_Team_Team2Id",
                        column: x => x.Team2Id,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateTable(
                name: "ScheduledMatch",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AppointmentId = table.Column<string>(nullable: true),
                    MatchId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledMatch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduledMatch_Match_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Match",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("FavoriteTeam");
            migrationBuilder.DropTable("ScheduledMatch");
            migrationBuilder.DropTable("Match");
            migrationBuilder.DropTable("League");
            migrationBuilder.DropTable("Team");
            migrationBuilder.DropTable("LeagueImage");
            migrationBuilder.DropTable("CountryImage");
            migrationBuilder.DropTable("TeamImage");
        }
    }
}
