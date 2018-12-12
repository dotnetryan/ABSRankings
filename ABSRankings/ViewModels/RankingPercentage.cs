using ABSRankings.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABSRankings.ViewModels
{
    public class OverallRankingPercentageDisplay
    {
        public List<RankingPercentageDisplay> RankingPercentages { get; set; }
        public OverallPercentageTrendDisplay OverallPercentageTrends { get; set; }
    }
    public class RankingPercentageDisplay
    {
        public int ID { get; set; }

        public Player Player { get; set; }

        [Display(Name = "Player Name")]
        public string PlayerName { get; set; }

        [Display(Name = "Encounter")]
        public string Encounter { get; set; }

        [Display(Name = "Difficulty")]
        public string Difficulty { get; set; }

        [Display(Name = "iLevel")]
        public int PlayerILevel { get; set; }

        [Display(Name = "Report URL")]
        public string ReportUrl { get; set; }

        [Display(Name = "Metric")]
        public string Metric { get; set; }

        public decimal Total { get; set; }

        [Display(Name = "Percentage")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal Percentage { get; set; }

        public string CssClass { get; set; }

        public List<RankingPercentageBySpecDisplay> SpecPercentages { get; set; }
    }

    public class RankingPercentageBySpecDisplay
    {
        public Player Player { get; set; }

        [Display(Name = "Percentage")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal Percentage { get; set; }
        public string CssClass { get; set; }
    }
}