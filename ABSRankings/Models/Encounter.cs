using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ABSRankings.Models
{
    public class Encounter
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Encounter Name")]
        public string Name { get; set; }
        public int WCLEncounterId { get; set; }

        [ForeignKey("Zone")]
        public int ZoneId { get; set; }
        public Zone Zone { get; set; }
    }
}