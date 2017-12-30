﻿using System;
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
        /// <summary>
        /// This is nullable just to allow ModelState validation with null or zero Id value.
        /// Will be valorized when SaveChanges is called.
        /// </summary>
        public int? StockId { get; set; }

        /// <summary>
        /// Represent the ingredient which the stock belongs to.
        /// </summary>
        public int IngredientId { get; set; }

        [Display(Name = "Quantità")]
        public int Quantity { get; set; }

        [Display(Name = "Scadenza")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ExpireDate { get; set; }

        [StringLength(1024)]
        [Display(Name = "Nota")]
        public string Note { get; set; }


        // Navigation Properties
        [Display(Name = "Ingrediente")]
        public virtual Ingredient Ingredient { get; set; }
    }
}