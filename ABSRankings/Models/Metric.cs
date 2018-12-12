using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ABSRankings.Models
{
    public class Metric
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Metric Type")]
        public string Type { get; set; }

        [Display(Name = "Metric Description")]
        public string Description { get; set; }
    }
}