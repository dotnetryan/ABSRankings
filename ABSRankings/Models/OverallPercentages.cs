using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ABSRankings.Models
{
    public class OverallPercentages
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Player")]
        [Required]
        public int PlayerId { get; set; }
        public Player Player { get; set; }

        [ForeignKey("Difficulty")]
        public int DifficultyId { get; set; }
        public Difficulty Difficulty { get; set; }

        [ForeignKey("Metric")]
        public int MetricId { get; set; }
        public Metric Metric { get; set; }

        [Required]
        public decimal Percentage { get; set; }

        [Display(Name = "Date Added")]
        [Required]
        public DateTime DateAdded { get; set; }
    }
}