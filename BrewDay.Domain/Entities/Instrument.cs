using BrewDay.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrewDay.Domain.Entities
{
    /// <summary>
    /// Represent an instrument owned by the Brewer
    /// </summary>
    
    public class Instrument
    {

        [Key]
        public int? InstrumentId { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name="Nome")]
        public string Name { get; set; }

        [StringLength(512)]
        [Display(Name = "Descrizione")]
        public string Description { get; set; }

        [Display(Name = "Quantità")] // There is no bound to the quantity of the element
        public int Quantity { get; set; }

        [Display(Name = "Capacità")]
        public double Capacity { get; set; }

        [Range(1, 3, ErrorMessage = "Devi selezionare un valore valido per il Tipo."), Display(Name = "Tipo")]
        public InstrumentType Type { get; set; }

        [Display(Name = "Utilizzati")]
        public int Used { get; set; } = 0;

        //Navigation Property
        public virtual ICollection<Production> Productions { get; set; }

        //public int Available { get { return Quantity - Used; } }
    }
}