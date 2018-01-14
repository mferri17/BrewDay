using System;
using System.Linq;
using System.Data.Entity;
using System.Web.Mvc;
using System.Collections.Generic;

using BrewDay.Domain;
using BrewDay.Domain.Entities;

namespace BrewDay.Controllers
{
    public class IngredientsController : Controller
    {
        private BrewDayContext db = new BrewDayContext();


        public ActionResult Index()
        {
            return View(db.Ingredients.ToList());
        }

        public ActionResult AddToStock(int? id)
        {
            if (id == null)
                throw new Exception("Id dell'elemento non specificato.");

            Ingredient element = db.Ingredients.Find(id);

            // check if exists and element with the specified Id
            if (element == null)
                throw new Exception($"L'Id {id} non corrisponde ad alcun elemento.");
            Stock fittizio = db.Stocks.Create();
            fittizio.Ingredient = element;
            return View("AddToStock", fittizio);
        }



        public ActionResult Details(int? id)
        {
            if (id == null)
                throw new Exception("Id dell'elemento non specificato.");
            
            Ingredient element = db.Ingredients.Find(id);

            // check if exists and element with the specified Id
            if (element == null)
                throw new Exception($"L'Id {id} non corrisponde ad alcun elemento.");

            return View(element);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
                throw new Exception("Id dell'elemento non specificato.");
            
            Ingredient element = db.Ingredients.Find(id);

            // check if exists and element with the specified Id
            if (element == null)
                throw new Exception($"L'Id {id} non corrisponde ad alcun elemento.");

            // deletes element from the context (marks it as "deleted")
            db.Ingredients.Remove(element);

            // takes changes from context and makes it real on the database
            db.SaveChanges();

            // if everything's ok, redirect to the index
            return RedirectToAction("Index");
        }
        

        public ActionResult CreateOrEdit(int? id)
        {
            // find by Id the element to edit, if exists
            Ingredient model = db.Ingredients.Find(id);

            // model can be valorized or even null
            return View("CreateOrEdit", model);
        }

        [HttpPost]
        public ActionResult CreateOrEdit(Ingredient model)
        {
            // check if all properties of the received model are valid
            if (ModelState.IsValid)
            {
                // id null or zero means model is new (not exist yet in db), so it has to be created
                if (!model.IngredientId.HasValue || model.IngredientId == 0)
                {
                    // add model to context and mark it as "added"
                    db.Ingredients.Add(model);
                }
                // valid id means model already exists in db, so it just has to be updated 
                else
                {
                    // mark this model as "modified" on the context
                    db.Entry(model).State = EntityState.Modified;
                }

                // takes changes from context and makes it real on the database
                db.SaveChanges();

                // if everything's ok, redirect to the index
                return RedirectToAction("Details", new { id = model.IngredientId });
            }
            // if model is not valid...
            else
            {
                // ... returns the same view again, with prefilled field (model previously received)
                return View("CreateOrEdit", model);
            }
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
