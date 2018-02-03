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
        [Display(Name = "Note")]
        public string Note { get; set; }

        [Display(Name = "Temp ferm.")]
        public double? FermentationTemperature { get; set; }

        //[Display(Name = "Id ricetta padre")]
        public int? ParentRecipeId { get; set; }

        [Required]
        [Display(Name="Durata")]
        [Range(0, Int32.MaxValue)] // valore minimo e max
        public int FermentationTime { get; set; }


        // Navigation Properties
        [Display(Name = "Ingredienti")]
        public virtual ICollection<RecipeIngredient> Ingredients { get; set; }

        [ForeignKey("ParentRecipeId")] // obbligatorio specificarlo perché essendo una self relationship altrimenti Entity Framework non lo capisce
        [Display(Name = "Ricetta Padre")]
        public virtual Recipe ParentRecipe { get; set; }
        [ForeignKey("ParentRecipeId")]
        public virtual ICollection<Recipe> Versions { get; set; }

        public virtual ICollection<Production> Productions { get; set; }



        // Utils
        [Display(Name = "Ricetta")]
        public string FullName { get { return ParentRecipe == null ? Name : ParentRecipe.Name + " " + Name; } }

        public bool HasProductions { get { return Productions != null ? Productions.Count > 0 : false; } }

    }
}