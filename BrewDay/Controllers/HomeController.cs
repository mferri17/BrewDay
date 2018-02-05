using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BrewDay.Domain;
using BrewDay.DTO;


namespace BrewDay.Controllers
{
    public class HomeController : Controller
    {
        private readonly BrewDayContext db = new BrewDayContext();

        public ActionResult Index()
        {
            return View(new ProductsAudit());
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult WhatShouldIBrewToday()
        {
            // ---------- Calcolo innanzitutto qual è la quantità di birra producibile sulla base degli Strumenti a sisposizione

            // calcolo capacità max per ogni tipo di strumento a disposizione ...
            var kettle = db.Instruments.Where(x => x.Type == Domain.Enums.InstrumentType.Kettle && (x.Quantity - x.Used) > 0).OrderByDescending(x => x.Capacity).FirstOrDefault()?.Capacity;
            var fermenter = db.Instruments.Where(x => x.Type == Domain.Enums.InstrumentType.Fermenter && (x.Quantity - x.Used) > 0).OrderByDescending(x => x.Capacity).FirstOrDefault()?.Capacity;
            var pipe = db.Instruments.Where(x => x.Type == Domain.Enums.InstrumentType.Pipe && (x.Quantity - x.Used) > 0).OrderByDescending(x => x.Capacity).FirstOrDefault()?.Capacity;

            // ... controllo se ne ho a disposizione ...
            if (kettle == null || fermenter == null || pipe == null)
                throw new InvalidOperationException("Non hai sufficienti Strumenti a disposizione.");

            // ... e dei tre considero quello con capacità minore
            int maxCapacity = (int)Math.Min(Math.Min(kettle.Value, fermenter.Value), pipe.Value); // litri max di birra producibile con gli strumenti a disposizione
            if (maxCapacity <= 0)
                throw new InvalidOperationException("Non hai Strumenti con una capacità sufficiente per produrre birra.");


            // ---------- Ora calcolo la quantità che possiedo di ciascun Ingrediente

            Dictionary<int, int> ownedIngredients = new Dictionary<int, int>(); // coppie <IngredientId, Quantity> degli ingredienti presenti in magazzino (stock)

            foreach (var stock in db.Stocks.Where(x => x.ExpireDate >= DateTime.Now)) // questo ciclo si potrebbe sostituire con una query LINQ con GroupBy + Select
            {
                if (ownedIngredients.ContainsKey(stock.IngredientId))
                    ownedIngredients[stock.IngredientId] += stock.Quantity;
                else
                    ownedIngredients.Add(stock.IngredientId, stock.Quantity);
            }

            if (ownedIngredients.Count == 0)
                throw new InvalidOperationException("Non possiedi alcuna Scorta in magazzino.");



            // ---------- Per ogni Ricetta calcolo quanto posso al massimo produrne con gli Stock e Strumenti a disposizione

            Dictionary<int, int> feasibleRecipes = new Dictionary<int, int>(); // coppie <RecipeId, Quantity> dei litri di ricetta producibili (con strumenti e stock a disposizione)

            foreach (var recipe in db.Recipes.ToList())
            {
                if (recipe.Ingredients == null || recipe.Ingredients.Count == 0) // ignoro ricette prive di ingredienti
                    continue;

                int feasible = Int32.MaxValue; // quantità fattibile delle Ricetta

                foreach (var ingredient in recipe.Ingredients)
                {
                    if (!ownedIngredients.ContainsKey(ingredient.IngredientId))
                    {
                        feasible = 0;
                        break; // non abbiamo questo ingrediente in magazzino (stock), pertanto non è possibile produrre tale ricetta
                    }

                    int tmp = ownedIngredients[ingredient.IngredientId] / ingredient.Quantity; // numero di produzioni per cui questo stock è sufficiente
                    if (tmp < feasible)
                        feasible = tmp; // aggiorno 
                }

                if (feasible != 0)
                {
                    if (feasible > maxCapacity) // non posso superare la maxCapacity
                        feasible = maxCapacity;
                    feasibleRecipes.Add(recipe.RecipeId.Value, feasible);
                }
            }


            WhatShouldIBrewToday result = null;

            if (feasibleRecipes.Count > 0)
            {
                var best = feasibleRecipes.OrderBy(x => x.Value).FirstOrDefault();
                result = new WhatShouldIBrewToday(db.Recipes.Find(best.Key), best.Value);
            }

            return View(result);
        }
    }
}