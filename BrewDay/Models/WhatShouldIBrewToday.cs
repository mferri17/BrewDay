using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BrewDay.Domain.Entities;

namespace BrewDay.DTO
{
    public class WhatShouldIBrewToday
    {
        public WhatShouldIBrewToday(Recipe recipe, int qty)
        {
            Recipe = recipe;
            Quantity = qty;
        }

        public Recipe Recipe { get; set; }
        public int? Quantity { get; set; }

    }
}