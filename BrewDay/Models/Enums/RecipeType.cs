using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrewDay.Models.Enums
{
    public enum RecipeType
    {
        [Display(Name = "Bollitore")]
        Kettle = 1,

        [Display(Name = "Fermentatore")]
        Fermenter = 2,

        [Display(Name = "Tubo")]
        Pipe = 3
    }
}