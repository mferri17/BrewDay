using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrewDay.Models.Entities
{

    public class Production
    {

        [Key]
        public int? ProductionId { get; set; }

        [Display(Name = "Data Inizio")]
        public DateTime DateStart { get; set; }

        [Display(Name = "Data Fine")]
        public DateTime DateEnd { get; set; }

        [Display(Name = "Data Fine Stimata")]
        public DateTime DateEndEstimated { get; set; }

        [StringLength(1024)]
        [Display(Name = "Nota")]
        public string Note { get; set; }

        //Navigation Properties
        public virtual ICollection<Instrument> Instrument { get; set; }



    }
}