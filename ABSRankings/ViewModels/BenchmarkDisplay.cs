using ABSRankings.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABSRankings.ViewModels
{
    public class BenchmarkDisplay
    {
        public Difficulty Difficulty { get; set; }
        public Metric Metric { get; set; }

        [DisplayFormat(DataFormatString = "{0:n}")]
        public decimal Value { get; set; }
    }
}