using ABSRankings.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Chart.Mvc.ComplexChart;
using Chart.Mvc.Extensions;

namespace ABSRankings.ViewModels
{
    public class PlayerDetailsDisplay
    {
        public Player Player { get; set; }
        public List<BenchmarkDisplay> Benchmarks { get; set; }
        public List<RankingPercentageDisplay> Rankings { get; set; }
        public OverallPercentageTrendDisplay OverallPercentageTrends { get; set; }
    }

    public class OverallPercentageTrendDisplay
    {
        public List<string> Labels { get; set; }
        public List<ComplexDataset> ComplexDataSets { get; set; }
    }
}