using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ABSRankings.Models
{
    public class Player
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Player Name")]
        [Required]
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:n}")]
        [Required]
        public decimal Sim { get; set; }

        [ForeignKey("Spec")]
        [Required]
        public int SpecId { get; set; }
        public Spec Spec { get; set; }

        // Metrics relevant to player
        [ForeignKey("Metric")]
        [Required]
        public ICollection<int> MetricIds { get; set; }

        public bool Status { get; set; }

        public ICollection<Ranking> Rankings { get; set; }

        [Display(Name = "Last Updated")]
        public DateTime LastUpdated { get; set; }
    }
}