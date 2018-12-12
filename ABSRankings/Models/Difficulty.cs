using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABSRankings.Models
{
    public class Difficulty
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Difficulty Name")]
        public string Name { get; set; }
        public int WCLDifficultyId { get; set; }
    }
}