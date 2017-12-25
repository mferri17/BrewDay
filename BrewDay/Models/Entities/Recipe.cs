using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrewDay.Models.Entities
{
    /// <summary>
    /// Represent a beer recipe that user can product.
    /// </summary>
    public class Recipe
    {
        [Key]
        public int RecipeId { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [StringLength(1024)]
        [Display(Name = "Descrizione")]
        public string Description { get; set; }

        [StringLength(1024)]
        [Display(Name = "Nota")]
        public string Note { get; set; }

        [Display(Name = "Temperatura di fermentazione")]
        public double? FermentationTemperature { get; set; }
        
        public int? ParentRecipeId { get; set; }


        // Navigation Properties
        public virtual Recipe ParentRecipe { get; set; }
        public virtual ICollection<Ingredient> Ingredients { get; set; }


        // Util Properties and Methods
        public string FullName { get { return ParentRecipe == null ? Name : ParentRecipe.Name + " " + Name; } }

    }
}