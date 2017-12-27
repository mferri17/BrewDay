using BrewDay.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrewDay.Models.Entities
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

        [Display(Name = "Quantità")] // There is no bound to the quantity of the element
        public double Quantity { get; set; }

        [Display(Name = "Capacità")]
        public double Capacity { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name = "Descrizione")]
        public string Description { get; set; }

        [Range(1, 3, ErrorMessage = "Devi selezionare un valore valido per il Tipo."), Display(Name = "Tipo")]
        public InstrumentType Type { get; set; }

        //Reminder: add a navigation properties to Production

    }
}