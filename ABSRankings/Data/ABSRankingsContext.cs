using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ABSRankings.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABSRankings.Data
{
    public class ABSRankingsContext : DbContext
    {
        public ABSRankingsContext() : base("ABSRankingsContext")
        {
            //TODO remove this in prod
            this.Database.CommandTimeout = 180;
        }

        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Encounter> Encounters { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Ranking> Rankings { get; set; }
        public DbSet<Spec> Specs { get; set; }
        public DbSet<Metric> Metrics { get; set; }
        public DbSet<Configuration> Configurations { get; set; }
        public DbSet<Coefficient> Coefficients { get; set; }
        public DbSet<OverallPercentages> OverallPercentages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}