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
        private BrewDayContext db = new BrewDayContext();

        // GET: Productions
        public ActionResult Index()
        {
            return View(db.Productions.ToList());
        }

        // GET: Productions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Production production = db.Productions.Find(id);
            if (production == null)
            {
                return HttpNotFound();
            }
            return View(production);
        }

        //GET: Productions/Play? qty = 50 & recipeId = 7
        public ActionResult Play(int? recipeId, int qty)
        {
            ViewBag.RecipeToPlay = recipeId;

            /*
             Per ogni ingrediente della ricetta che voglio mandare in produzione devo controllare:
             - Gli ingredienti in Stock siano, in numero, >= rispetto a quelli richiesti
             - Gli ingredienti necessari NON siano scaduti
             - Controllare che ci siano abbastanza Instrument
             */
             
            if (!recipeId.HasValue)
                throw new Exception("Id non specificato.");

            Recipe recipe = db.Recipes.Find(recipeId);

            if (recipe == null)
                throw new Exception("Non esiste una ricetta con questo Id.");

            var ingredients = recipe.Ingredients;

            //var result = db.Stocks.Where(x => x.Ingredient.Name.Contains("bello")).ToList();

            foreach (var element in ingredients)
            {
                #region appunti
                //var qtyNeeded = 10;

                //// SELECT Sum(Quantity) FROM Stocks WHERE IngredientId == element.IngredientId
                //var owned = db.Stocks.Where(x => x.IngredientId == element.IngredientId).Select(x => x.Quantity).Sum();

                //if (owned >= qtyNeeded)
                //{
                //    //OK
                //}
                //else
                //{
                //    //ERRORE
                //}
                #endregion
                var QuantityNeeded = element.Quantity * qty; //Quantità richiesta per elemento

                var IngredientInStock = db.Stocks.Where(x => x.IngredientId == element.IngredientId && x.ExpireDate >= DateTime.Now); //Ingrediente che ci serve
                int? QuantityStock = IngredientInStock.Select(x => x.Quantity).DefaultIfEmpty(0).Sum(); //Quantità di quell'ingrediente nello Stock
                if (QuantityStock == null || QuantityNeeded > QuantityStock)
                {
                    throw new Exception("Quantità non sufficiente o ingredienti scaduti");
                }
                
                // Prendo la data di scadenza dell'elemento corrente
                var ExpDateEl = IngredientInStock.Select(x => x.ExpireDate).FirstOrDefault();//First() perchè, se no,sarebbe una collection

                #region appunti
                // ATTENTI CHE COSì PRENDETE LA SCADENZA DI UN SOLO STOCK, MA PER LA QUANTITà RICHEISTA POTRESTE AVER BISOGNO DI SFRUTTARE PIù STOCK, QUINDI DOVETE VERIFICARE CHE TUTTI QUELLI RICHIESTI NON SIANO SCADUTI
                //if (DateTime.Now > ExpDateEl)
                //{
                //    throw new Exception("Ingrediente scaduto");
                //}

                // qui per ogni stock dovete scalare la quantità che utilizzate
                #endregion
            }

            // var prova = db.Instruments.Where(x => x.Name == "Kettle" & x.Production.Contains(db.Instruments.Where(y => y.InstrumentId == x.InstrumentId));
            var Kettle = db.Instruments.Where(x => x.Type == Domain.Enums.InstrumentType.Kettle && (x.Quantity - x.Used) < x.Quantity).FirstOrDefault(); //Select(x => x.InstrumentId) non necessario
            var Fermenter = db.Instruments.Where(x => x.Type == Domain.Enums.InstrumentType.Fermenter && (x.Quantity - x.Used) < x.Quantity).FirstOrDefault();
            var Pipe = db.Instruments.Where(x => x.Type == Domain.Enums.InstrumentType.Pipe && (x.Quantity - x.Used) < x.Quantity).FirstOrDefault();

            if (Kettle == null || Fermenter == null || Pipe == null)
            {
                throw new Exception("Uno o più strumenti richiesti non disponibili");
            }

            //Mandando in produzione la Ricetta, la quantità di ogni ingrediente necessario per la stessa, andrà scalato da quella attuale in magazzino
            foreach (var element in ingredients)
            {
                var QuantityNeeded = element.Quantity; //Quantità richiesta per elemento
                var IngredientInStock = db.Stocks.Where(x => x.IngredientId == element.IngredientId).FirstOrDefault(); //Ingrediente effettivo in Stock
                IngredientInStock.Quantity = IngredientInStock.Quantity - QuantityNeeded;
                db.Entry(IngredientInStock).State = EntityState.Modified;
            }

            //Gli Strumenti necessari per la produzione andranno resi "Impegnati"
            Kettle.Used++;
            Fermenter.Used++;
            Pipe.Used++;  //true è usato

            // Rendiamo effettive le Modifiche
            db.Entry(Kettle).State = EntityState.Modified;
            db.Entry(Fermenter).State = EntityState.Modified;
            db.Entry(Pipe).State = EntityState.Modified;
            db.SaveChanges();

            var RecipeToPush = db.Recipes.Find(recipeId);

            Production Production = new Production();

            Production.DateStart = DateTime.Now;
            Production.DateEnd = null;
            Production.DateEndEstimated = DateTime.Now.AddDays(RecipeToPush.FermentationTime);
            Production.Note = RecipeToPush.Note;

            db.Productions.Add(Production);
            db.SaveChanges();


            
            
            return RedirectToAction("Index");
        }

        // POST: Productions/Create
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductionId,DateStart,DateEnd,DateEndEstimated,Note")] Production production)
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
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Production production = db.Productions.Find(id);
            if (production == null)
            {
                return HttpNotFound();
            }
            return View(production);
        }

        // POST: Productions/Edit/5
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductionId,DateStart,DateEnd,DateEndEstimated,Note")] Production production)
        {
            if (ModelState.IsValid)
            {
                db.Entry(production).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(production);
        }

        // GET: Productions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Production production = db.Productions.Find(id);
            if (production == null)
            {
                return HttpNotFound();
            }
            return View(production);
        }

        // POST: Productions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Production production = db.Productions.Find(id);
            db.Productions.Remove(production);
            db.SaveChanges();
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
