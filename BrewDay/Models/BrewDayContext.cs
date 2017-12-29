using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BrewDay.Models
{
    public class BrewDayContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public BrewDayContext() : base("name=BrewDayContext")
        {
        }

        public System.Data.Entity.DbSet<BrewDay.Models.Entities.Ingredient> Ingredients { get; set; }

        public System.Data.Entity.DbSet<BrewDay.Models.Entities.Instrument> Instruments { get; set; }

        public System.Data.Entity.DbSet<BrewDay.Models.Entities.Recipe> Recipe { get; set; }

        public System.Data.Entity.DbSet<BrewDay.Models.Entities.Stock> Stocks { get; set; }

        public System.Data.Entity.DbSet<BrewDay.Models.Entities.Production> Production { get; set; }
    }
}
