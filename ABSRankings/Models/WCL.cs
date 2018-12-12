using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABSRankings.Models.WCL
{
    public class Ranking
    {
        public int Encounter { get; set; }
        public int Class { get; set; }
        public int Spec { get; set; }
        public string Guild { get; set; }
        public int Rank { get; set; }
        public int OutOf { get; set; }
        public string Duration { get; set; }
        public string StartTime { get; set; }
        public string ReportId { get; set; }
        public int FightId { get; set; }
        public int Difficulty { get; set; }
        public int Size { get; set; }
        public int ItemLevel { get; set; }
        public decimal Total { get; set; }
        public bool Estimated { get; set; }
    }

    public class Encounter
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Zone
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Frozen { get; set; }
        public ICollection<Encounter> Encounters { get; set; }
    }

    public class Spec
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Class
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Spec> Specs { get; set; }
    }
}