using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewDay.Domain.Entities
{
    /// <summary>
    /// Represent a beer recipe that user can product.
    /// </summary>
    public class Recipe
    {
        [Key]
        /// <summary>
        /// This is nullable just to allow ModelState validation with null or zero Id value.
        /// Will be valorized when SaveChanges is called.
        /// </summary>
        public int? RecipeId { get; set; }

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

        //[Display(Name = "Id ricetta padre")]
        public int? ParentRecipeId { get; set; }


        // Navigation Properties
        public virtual ICollection<RecipeIngredient> Ingredients { get; set; }

        [ForeignKey("ParentRecipeId")]
        public virtual Recipe ParentRecipe { get; set; }


        // Util Properties and Methods
        public string FullName { get { return ParentRecipe == null ? Name : ParentRecipe.Name + " " + Name; } }

    }
}