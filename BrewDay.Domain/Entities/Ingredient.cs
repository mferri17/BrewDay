using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using BrewDay.Domain.Enums;

namespace BrewDay.Domain.Entities
{
    /// <summary>
    /// Represent an ingredient commercially available, to make recipes with.
    /// </summary>
    public class Ingredient
    {
        [Key]
        /// <summary>
        /// This is nullable just to allow ModelState validation with null or zero Id value.
        /// Will be valorized when SaveChanges is called.
        /// </summary>
        public int? IngredientId { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name = "Nome")]
        public string Name { get; set; }
        
        [StringLength(1024)]
        [Display(Name = "Descrizione")]
        public string Description { get; set; }

        [Display(Name = "Prezzo")]
        [DataType(DataType.Currency)] // Influenza il modo in cui questa viene visualizzata con una DisplayFor
        public double? Price { get; set; }

        [Range(1, 4, ErrorMessage = "Devi selezionare un valore valido per il Tipo."), Display(Name = "Tipo")]
        public IngredientType Type { get; set; }


        // Navigation Properties
        public virtual ICollection<Stock> Stocks { get; set; }
        public virtual ICollection<RecipeIngredient> Recipes { get; set; }


        // Utils
        [Display(Name = "Nome Ingrediente")]
        public string FullName { get { return Type.ToString() + " - " + Name; } }

    }
}