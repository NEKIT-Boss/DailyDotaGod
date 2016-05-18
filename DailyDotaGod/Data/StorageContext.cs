using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyDotaGod.Data
{
    class StorageContext : DbContext
    {
        public DbSet<Match> Matches { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<League> Leagues { get; set; }

        // Need to think whether or not we need it, and we can, I think hide it.
        public DbSet<LeagueImage> LeagueImages { get; set; }
        public DbSet<TeamImage> TeamImages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Storage.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Match>()
                .Property(entity => entity.Id)
                .IsRequired();

            modelBuilder.Entity<Team>()
                .Property( entity => entity.Id)
                .IsRequired();

            modelBuilder.Entity<League>()
                    .Property(entity => entity.Id)
                    .IsRequired();
        }
    }
}
