using System;
using System.Linq;
using System.Data.Entity;

using BrewDay.Domain.Entities;

namespace BrewDay.Domain
{
    public class BrewDayContext : DbContext
    {
        private static BrewDayContext BrewDayContext_instance = null;
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        /*public BrewDayContext() : base("name=BrewDayContext")
        {

        }*/

        /* public BrewDayContext()
         {

             if (BrewDayContext_instance == null)
                 BrewDayContext_instance = new BrewDayContext();

             return BrewDayContext_instance;
         }*/

        public static BrewDayContext Instance
        {
            get
            {
                if (BrewDayContext_instance == null)
                {                    
                        if (BrewDayContext_instance == null)
                        BrewDayContext_instance = new BrewDayContext();
                   
                }
                return BrewDayContext_instance;
            }
        }



    public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<Instrument> Instruments { get; set; }
        public DbSet<Production> Productions { get; set; }
        public DbSet<Stock> Stocks { get; set; }
    }
}
