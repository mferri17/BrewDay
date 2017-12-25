using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BrewDay.Models.Enums;

namespace BrewDay.Models.Entities
{
    /// <summary>
    /// Represent an ingredient commercially available, to make recipes with.
    /// </summary>
    public class Ingredient
    {
        [Key]
        public int IngredientId { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [StringLength(1024)]
        [Display(Name = "Descrizione")]
        public string Description { get; set; }

        [Display(Name = "Prezzo")]
        public double? Price { get; set; }

        [Range(1, 4, ErrorMessage = "Devi selezionare un valore valido per il Tipo."), Display(Name = "Tipo")]
        public IngredientType Type { get; set; }


        // Navigation Properties
        public virtual ICollection<Stock> Stocks { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }

    }
}