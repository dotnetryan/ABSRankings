using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABSRankings.Models
{
    public class Zone
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Zone Name")]
        public string Name { get; set; }
        public int WCLZoneId { get; set; }
        public bool WCLFrozen { get; set; }

        public ICollection<Encounter> Encouters { get; set; }
    }
}