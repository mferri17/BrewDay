using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BrewDay.Models.Enums
{
    public enum IngredientType
    {
        [Display(Name = "Malto")]
        Malt = 1,

        [Display(Name = "Luppolo")]
        Hop = 2,

        [Display(Name = "Lievito")]
        Yeast = 3,

        [Display(Name = "Additivo")]
        Additive = 4
    }
}