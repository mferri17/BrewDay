using System;
using System.Net;
using System.Linq;
using System.Data.Entity;
using System.Web.Mvc;
using System.Collections.Generic;

using BrewDay.Domain;
using BrewDay.Domain.Entities;

namespace BrewDay.Controllers
{
    public class ProductionsController : Controller
    {
        private readonly BrewDayContext db = new BrewDayContext();

        // GET: Productions
        public ActionResult Index()
        {
            return View(db.Productions.ToList());
        }

        // GET: Productions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                throw new MissingIdBrewDayException();

            Production production = db.Productions.Find(id);
            if (production == null)
                throw new InvalidIdBrewDayException(id.Value);

            return View(production);
        }

        // Productions/Play? qty = 50 & recipeId = 7
        public ActionResult Play(int? recipeId, int? qty)
        {
            /*
                Per ogni ingrediente della ricetta che voglio mandare in produzione devo controllare:
                - Gli ingredienti in Stock siano, in numero, >= rispetto a quelli richiesti
                - Gli ingredienti necessari NON siano scaduti
                - Controllare che ci siano abbastanza Instrument
            */

            if (!recipeId.HasValue)
                throw new MissingIdBrewDayException();

            if (!qty.HasValue)
                throw new InvalidOperationBrewDayException("Devi specificare una quantità.");

            Recipe recipe = db.Recipes.Find(recipeId);
            if (recipe == null)
                throw new InvalidIdBrewDayException(recipeId.Value);

            var ingredients = recipe.Ingredients;
            try
            {
                // Prima di selezionare i seguenti strumenti si ordina per capacità in modo da selezionare quello che spreca meno capacità (best fit)
                var kettle = db.Instruments.Where(x => x.Type == Domain.Enums.InstrumentType.Kettle && (x.Quantity - x.Used) > 0 && x.Capacity > qty).OrderBy(x => x.Capacity).FirstOrDefault();
                var fermenter = db.Instruments.Where(x => x.Type == Domain.Enums.InstrumentType.Fermenter && (x.Quantity - x.Used) > 0 && x.Capacity > qty).OrderBy(x => x.Capacity).FirstOrDefault();
                var pipe = db.Instruments.Where(x => x.Type == Domain.Enums.InstrumentType.Pipe && (x.Quantity - x.Used) > 0 && x.Capacity > qty).OrderBy(x => x.Capacity).FirstOrDefault();

                if (kettle == null || fermenter == null || pipe == null)
                    throw new InvalidOperationBrewDayException("Uno o più strumenti richiesti non sono disponibili. É necessario possedere un bollitore, un fermentatore e un tubo della capacità adatta.");


                // Controllo che vi siano a disposizione tutti gli Ingredienti necessari e in caso positivo scalo la quantità dai rispettivi Stock
                foreach (var element in ingredients)
                {
                    var requestedQuantity = element.Quantity * qty.Value; // quantità richiesta per questo ingrediente

                    // le due istruzioni sotto sono equivalenti alla query: SELECT Sum(Quantity) FROM Stocks WHERE IngredientId == element.IngredientId AND ExpireDate >= DateTime.Now AND Quantity > 0
                    var ingredientStocks = db.Stocks.Where(x => x.IngredientId == element.IngredientId && x.ExpireDate >= DateTime.Now && x.Quantity > 0); // lista scorte (disponibili) per questo ingrediente
                    int? ownedQuantity = ingredientStocks.Select(x => x.Quantity).DefaultIfEmpty(0).Sum(); // quantità di questo ingrediente presente in magazzino

                    if (ownedQuantity == null || requestedQuantity > ownedQuantity)
                        throw new InvalidOperationBrewDayException($"Quantità di {element.Ingredient.FullName} non sufficiente in magazzino. Controllare anche che non siano scaduti."); // notare il $ prima della stringa ("c# string interpolation" su google)


                    // Mandando in produzione la Ricetta, la quantità di ogni ingrediente necessario per la stessa andrà scalata da quella attuale in magazzino
                    // Potrebbero verificarsi dei casi in cui un singolo Stock non è sufficiente a coprire la quantità necessaria, quindi si attinge a più Stock diversi dello stesso Ingrediente

                    int demand = qty.Value; // domanda, scalata di volta in volta
                    foreach(var stock in ingredientStocks.OrderBy(x => x.ExpireDate)) // ordinati per data di scadenza (best fit)
                    {
                        if(stock.Quantity > demand) { // lo stock soddisfa il fabbisogno dell'ingrediente
                            stock.Quantity -= demand; // scalo quantità usata dallo stock
                            db.Entry(stock).State = EntityState.Modified;
                            demand = 0;
                            break; // ciclo terminato
                        }

                        else if(stock.Quantity <= demand) // lo stock non è da solo sufficiente a soddisfare il fabbisogno dell'ingrediente
                        {
                            demand -= stock.Quantity; // sfrutto tutta la quantità di questo stock e passo al prossimo ciclo
                            db.Entry(stock).State = EntityState.Deleted; // la quantità di questo stock passerebbe a zero, tanto vale cancellarlo

                            // Questa "cancellazione" non dà problemi nel caso si verifichino eccezioni sul cicli successivi del foreach più esterno
                            // perché in realtà settando un Entry come Deleted, essa non è ancora stata cancellata effettivamente dal db
                            // verrà cancellata solo al prossimo SaveChanges(), che avviene alla fine del metodo, se tutto è andato bene.
                        }
                    }

                    if(demand != 0) // questo controllo è in realtà ridondante (viene controllato già prima del foreach che gli stock siano sufficienti), ma non si sa mai
                        throw new InvalidOperationBrewDayException($"Quantità di {element.Ingredient.FullName} non sufficiente in magazzino. Controllare anche che non siano scaduti.");
                }

                // Aggiornamento nel db degli Strumenti utilizzati
                var usedIntruments = new Instrument[] { kettle, pipe, fermenter };
                foreach (var instrument in usedIntruments)
                {
                    instrument.Used++;
                    db.Entry(instrument).State = EntityState.Modified;
                }

                // Creazione della Produzione
                Production production = new Production()
                {
                    RecipeId = recipe.RecipeId.Value,
                    DateStart = DateTime.Now,
                    DateEnd = null,
                    DateEndEstimated = DateTime.Now.AddDays(recipe.FermentationTime),
                    Note = recipe.Note,
                    Instruments = usedIntruments,
                };
                db.Productions.Add(production);

                // Solo qui rendo effettivi i cambiamenti sul database, se non vi sono state precedenti eccezioni
                db.SaveChanges();
                return RedirectToAction("Details", "Productions", new { id = production.ProductionId });
            }

            catch (Exception e)
            {
                /*  Scenari di fallimento del PlayProduction:
                        uno o più strumenti richiesti non sono disponibili
                        quantità di uno o più ingredienti non sufficiente in magazzino
                        (errori di programmazione)
                */
                TempData["Message"] = "Errore! " + e.Message;
                return RedirectToAction("Details", "Recipes", new { id = recipeId });
            }
        }

        public ActionResult Stop(int? id)
        {
            if (!id.HasValue)
                throw new MissingIdBrewDayException();

            //Cerco la produzione
            var Production = db.Productions.Find(id);

            if (Production == null)
                throw new InvalidIdBrewDayException(id.Value);

            foreach (Instrument instrument in Production.Instruments)
            {
                instrument.Used--;
                db.Entry(instrument).State = EntityState.Modified;
                db.SaveChanges();
            }

            //Imposto la data di fine fermentazione
            Production.DateEnd = DateTime.Now;
            db.Entry(Production).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");

        }

        // POST: Productions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Production production)
        {
            if (ModelState.IsValid)
            {
                db.Productions.Add(production);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(production);
        }

        // GET: Productions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                throw new MissingIdBrewDayException();

            Production production = db.Productions.Find(id);
            if (production == null)
                throw new InvalidIdBrewDayException(id.Value);

            return View(production);
        }

        // POST: Productions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Production production)
        {
            if (ModelState.IsValid)
            {
                db.Entry(production).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = production.ProductionId });
            }
            return View(production);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
                throw new MissingIdBrewDayException();

            Production production = db.Productions.Find(id);

            if (production == null)
                throw new InvalidIdBrewDayException(id.Value);


            // sets the used instrument as no more used only if the production is still running
            if (production.Running)
            {
                foreach (Instrument inst in production.Instruments)
                {
                    inst.Used--;
                    db.Entry(inst).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            // deletes element from the context (marks it as "deleted")
            db.Productions.Remove(production);

            // takes changes from context and makes it real on the database
            db.SaveChanges();

            // if everything's ok, redirect to the index
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
