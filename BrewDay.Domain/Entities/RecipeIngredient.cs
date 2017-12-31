﻿using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrewDay.Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class RecipeIngredient
    {
        [Column(Order = 0), Key]
        public int RecipeId { get; set; }

        [Column(Order = 1), Key]
        public int IngredientId { get; set; }

        public int Quantity { get; set; }


        // Navigation Properties

        public virtual Recipe Recipe { get; set; }
        public virtual Ingredient Ingredient { get; set; }
    }
}