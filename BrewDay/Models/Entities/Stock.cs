using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrewDay.Models.Entities
{
    /// <summary>
    /// Represent an ingredient stock with relative quantity and expire date.
    /// </summary>
    public class Stock
    {
        [Key]
        public int StockId { get; set; }
        
        public int IngredientId { get; set; }

        [Display(Name = "Quantità")]
        public int Quantity { get; set; }

        [Display(Name = "Scadenza")]
        public DateTime ExpireDate { get; set; }


        // Navigation Properties
        public virtual Ingredient Ingredient { get; set; }
    }
}