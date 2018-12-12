using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ABSRankings.Models;
using ABSRankings.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ABSRankings.Data
{
    public class DataInitializer : CreateDatabaseIfNotExists<ABSRankingsContext>
    {
        private WCLService wclService = new WCLService();

        protected override void Seed(ABSRankingsContext db)
        {
            //configurations keys
            var configruations = new List<Configuration> {
                new Configuration { Key = "apiKey", Value = "e1079063b036f448aee00c842c274900" },
                new Configuration { Key = "apiUrl", Value = "https://www.warcraftlogs.com/v1" },
                new Configuration { Key = "wowServer", Value = "Bleeding-Hollow" },
                new Configuration { Key = "wclUrl", Value = "https://www.warcraftlogs.com" },
                new Configuration { Key = "wclReportUrl", Value = "https://www.warcraftlogs.com/reports" }
            };
            configruations.ForEach(c => db.Configurations.Add(c)); //TODO add to repo
            db.SaveChanges();

            //metrics
            var dps = new Metric { Type = "dps", Description = "DPS" };
            var bossDps = new Metric { Type = "bossdps", Description = "Weighted DPS" };
            var hps = new Metric { Type = "hps", Description = "HPS" };
            var tankHps = new Metric { Type = "tankhps", Description = "Tank HPS" };
            var krsi = new Metric { Type = "krsi", Description = "Tank Survivability" };

            var metrics = new List<Metric>
            {
                dps,
                bossDps,
                hps,
                tankHps,
                krsi
            };

            metrics.ForEach(d => db.Metrics.Add(d)); //TODO add to repo
            db.SaveChanges();

            //difficulties
            var mythic = new Difficulty { Name = "Mythic", WCLDifficultyId = 5 };
            var heroic = new Difficulty { Name = "Heroic", WCLDifficultyId = 4 };
            var normal = new Difficulty { Name = "Normal", WCLDifficultyId = 3 };
            var lfr = new Difficulty { Name = "LFR", WCLDifficultyId = 1 };

            var difficulties = new List<Difficulty>
            {
                mythic,
                heroic
                //normal,
                //lfr
            };

            difficulties.ForEach(d => db.Difficulties.Add(d)); //TODO add to repo
            db.SaveChanges();

            // coefficients
            var coefficients = new List<Coefficient>()
            {
                new Coefficient { Difficulty = mythic, Metric = dps, Value = 0.9m },
                new Coefficient { Difficulty = heroic, Metric = dps, Value = 0.75m },
                //new Coefficient { Difficulty = normal, Metric = dps, Value = 0.65m },
                //new Coefficient { Difficulty = lfr, Metric = dps, Value = 0.5m },

                new Coefficient { Difficulty = mythic, Metric = bossDps, Value = 0.9m },
                new Coefficient { Difficulty = heroic, Metric = bossDps, Value = 0.75m },
                //new Coefficient { Difficulty = normal, Metric = bossDps, Value = 0.65m },
                //new Coefficient { Difficulty = lfr, Metric = bossDps, Value = 0.5m },

                new Coefficient { Difficulty = mythic, Metric = hps, Value = 0.9m },
                new Coefficient { Difficulty = heroic, Metric = hps, Value = 0.75m },
                //new Coefficient { Difficulty = normal, Metric = hps, Value = 0.65m },
                //new Coefficient { Difficulty = lfr, Metric = hps, Value = 0.5m },

                new Coefficient { Difficulty = mythic, Metric = tankHps, Value = 0.9m },
                new Coefficient { Difficulty = heroic, Metric = tankHps, Value = 0.75m },
                //new Coefficient { Difficulty = normal, Metric = tankHps, Value = 0.65m },
                //new Coefficient { Difficulty = lfr, Metric = tankHps, Value = 0.5m },

                new Coefficient { Difficulty = mythic, Metric = krsi, Value = 0.9m },
                new Coefficient { Difficulty = heroic, Metric = krsi, Value = 0.75m },
                //new Coefficient { Difficulty = normal, Metric = krsi, Value = 0.65m },
                //new Coefficient { Difficulty = lfr, Metric = krsi, Value = 0.5m }
            };

            coefficients.ForEach(d => db.Coefficients.Add(d)); //TODO add to repo
            db.SaveChanges();

            //zones and encounters
            var zones = wclService.GetZones();

            foreach (var zone in zones)
            {
                var newZone = new Zone { Name = zone.Name, WCLZoneId = zone.Id, WCLFrozen = zone.Frozen };
                db.Zones.Add(newZone); //TODO add to repo
                db.SaveChanges();

                foreach (var encounter in zone.Encounters)
                {
                    db.Encounters.Add(new Encounter { Name = encounter.Name, WCLEncounterId = encounter.Id, Zone = newZone }); //TODO add to repo
                }
                db.SaveChanges(); //TODO add to repo
            }

            //specs
            var classes = wclService.GetClasses();

            foreach (var wclClass in classes)
            {
                foreach (var spec in wclClass.Specs)
                {
                    db.Specs.Add(new Spec { Name = spec.Name, WCLSpecId = spec.Id, ClassName = wclClass.Name, WCLClassId = wclClass.Id }); //TODO add to repo
                }
            }
            db.SaveChanges(); //TODO add to repo

            var identityContext = new ApplicationDbContext();
            AddRole(identityContext);

            db.SaveChanges();
        }

        bool AddRole(ApplicationDbContext identityContext)
        {
            IdentityResult ir;
            var rm = new RoleManager<IdentityRole>
                (new RoleStore<IdentityRole>(identityContext));

            ir = rm.Create(new IdentityRole("admin"));

            return ir.Succeeded;
        }
    }
}