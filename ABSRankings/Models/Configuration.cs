using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace ABSRankings.Models
{
    public class Configuration
    {
        [Key]
        public int ID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}