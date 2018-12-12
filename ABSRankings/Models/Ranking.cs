using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ABSRankings.Models
{
    public class Ranking
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Player")]
        public int PlayerId { get; set; }
        public Player Player { get; set; }

        [ForeignKey("Encounter")]
        public int EncounterId { get; set; }
        public Encounter Encounter { get; set; }

        [ForeignKey("Difficulty")]
        public int DifficultyId { get; set; }
        public Difficulty Difficulty { get; set; }

        [ForeignKey("Metric")]
        public int MetricId { get; set; }
        public Metric Metric { get; set; }

        public string WCLReportId { get; set; }
        public int WCLFightId { get; set; }

        public int PlayerILevel { get; set; }

        public decimal Total { get; set; }
    }
}