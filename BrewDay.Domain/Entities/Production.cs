using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrewDay.Domain.Entities
{

    public class Production
    {

        [Key]
        public int? ProductionId { get; set; }

        public int RecipeId { get; set; }

        [Display(Name = "Data Inizio")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateStart { get; set; }

        [Display(Name = "Data Fine")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DateEnd { get; set; }

        [Display(Name = "Data Fine Stimata")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateEndEstimated { get; set; }

        [StringLength(1024)]
        [Display(Name = "Nota")]
        public string Note { get; set; }

        //public ICollection<Ingredient> Ingredients { get; set; }


        // Navigation Properties
        public virtual ICollection<Instrument> Instruments { get; set; }
        public virtual Recipe Recipe { get; set; }

    }
}