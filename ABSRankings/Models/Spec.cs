using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABSRankings.Models
{
    public class Spec
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Spec")]
        public string Name { get; set; }
        public int WCLSpecId { get; set; }

        [Display(Name = "Class")]
        public string ClassName { get; set; }
        public int WCLClassId { get; set; }
    }
}