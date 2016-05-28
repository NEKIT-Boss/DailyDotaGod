using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using DailyDotaGod.Data;

namespace DailyDotaGod.Migrations
{
    [DbContext(typeof(StorageContext))]
    [Migration("20160528061248_FavoriteTeamAdded")]
    partial class FavoriteTeamAdded
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348");

            modelBuilder.Entity("DailyDotaGod.Data.CountryImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<byte[]>("Data");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("DailyDotaGod.Data.FavoriteTeam", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Points");

                    b.Property<int?>("TeamId");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("DailyDotaGod.Data.League", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("LogoId");

                    b.Property<string>("Name");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("DailyDotaGod.Data.LeagueImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Data");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("DailyDotaGod.Data.Match", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BestOf");

                    b.Property<int?>("LeagueId");

                    b.Property<int>("LiveStatus");

                    b.Property<DateTime>("StartTime");

                    b.Property<int?>("Team1Id");

                    b.Property<int?>("Team2Id");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("DailyDotaGod.Data.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CountryLogoId");

                    b.Property<int?>("LogoId");

                    b.Property<string>("Name");

                    b.Property<string>("Tag");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("DailyDotaGod.Data.TeamImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Data");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("DailyDotaGod.Data.FavoriteTeam", b =>
                {
                    b.HasOne("DailyDotaGod.Data.Team")
                        .WithMany()
                        .HasForeignKey("TeamId");
                });

            modelBuilder.Entity("DailyDotaGod.Data.League", b =>
                {
                    b.HasOne("DailyDotaGod.Data.LeagueImage")
                        .WithMany()
                        .HasForeignKey("LogoId");
                });

            modelBuilder.Entity("DailyDotaGod.Data.Match", b =>
                {
                    b.HasOne("DailyDotaGod.Data.League")
                        .WithMany()
                        .HasForeignKey("LeagueId");

                    b.HasOne("DailyDotaGod.Data.Team")
                        .WithMany()
                        .HasForeignKey("Team1Id");

                    b.HasOne("DailyDotaGod.Data.Team")
                        .WithMany()
                        .HasForeignKey("Team2Id");
                });

            modelBuilder.Entity("DailyDotaGod.Data.Team", b =>
                {
                    b.HasOne("DailyDotaGod.Data.CountryImage")
                        .WithMany()
                        .HasForeignKey("CountryLogoId");

                    b.HasOne("DailyDotaGod.Data.TeamImage")
                        .WithMany()
                        .HasForeignKey("LogoId");
                });
        }
    }
}
