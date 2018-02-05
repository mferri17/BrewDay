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

        [Display(Name = "Fine Stimata")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateEndEstimated { get; set; }

        [Display(Name = "Quantità")]
        [Range(1, Int32.MaxValue)]
        public int Quantity { get; set; }

        [StringLength(1024)]
        [Display(Name = "Nota")]
        public string Note { get; set; }

       
        // Navigation Properties
        public virtual ICollection<Instrument> Instruments { get; set; }
        public virtual Recipe Recipe { get; set; }


        // Utils

        public string FullName => $"{Recipe.FullName} del {DateStart.ToShortDateString()}";

        public bool Running => DateEnd == null;
        public bool AlmostFinished => DateEnd == null && DateEndEstimated < DateTime.Now.AddDays(2);
        public bool Completed => !Running;

    }
}