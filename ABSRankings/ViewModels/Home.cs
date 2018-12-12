using ABSRankings.Models;
using System.Collections.Generic;

namespace ABSRankings.ViewModels
{
    public class HomeMatrixDisplay
    {
        public Difficulty Difficulty { get; set; }
        public List<Metric> Metrics { get; set; }
    }
}