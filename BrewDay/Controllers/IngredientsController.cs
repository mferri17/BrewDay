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
        private readonly BrewDayContext db = new BrewDayContext();


        public ActionResult Index()
        {
            return View(db.Ingredients.ToList());
        }
        

        public ActionResult Details(int? id)
        {
            if (id == null)
                throw new MissingIdBrewDayException();
            
            Ingredient element = db.Ingredients.Find(id);
            
            if (element == null)
                throw new InvalidIdBrewDayException(id.Value);

            return View(element);
        }
        

        public ActionResult Delete(int? id)
        {
            if (id == null)
                throw new MissingIdBrewDayException();

            Ingredient element = db.Ingredients.Find(id);
            
            if (element == null)
                throw new InvalidIdBrewDayException(id.Value);

            if(element.Recipes.Count > 0)
                throw new InvalidOperationBrewDayException("Non puoi cancellare un Ingrediente che viene usato in qualche Ricetta.");

            // cancella un elemento dal contesto (lo marchia come "cancellato")
            db.Ingredients.Remove(element);

            // prende i cambiamenti dal contesto e li apporta sull'effettivo database
            db.SaveChanges();

            // se tutto è ok, redirect all'index
            return RedirectToAction("Index");
        }
        
        public ActionResult CreateOrEdit(int? id)
        {
            Ingredient model = db.Ingredients.Find(id);
            
            // model può essere sia valorizzato che null
            return View("CreateOrEdit", model);
        }

        [HttpPost]
        public ActionResult CreateOrEdit(Ingredient model)
        {
            // controlla che tutte le proprietà del model siano valide, pescando dai tipi e dalle [DataAnnotation] della classe Ingredient
            if (ModelState.IsValid)
            {
                // se l'Id è null o uguale a 0, significa che l'elemento non esiste nel datagbase e bisogna crearlo
                if (!model.IngredientId.HasValue || model.IngredientId == 0)
                {
                    // aggiunge model al contesto (lo marchia come "aggiunto")
                    db.Ingredients.Add(model);
                }
                // valid id means model already exists in db, so it just has to be updated 
                else
                {
                    // marchio il model come "modificato" all'interno del contesto
                    db.Entry(model).State = EntityState.Modified;
                }

                // prende i cambiamenti dal contesto e li apporta sull'effettivo database
                db.SaveChanges();

                // se tutto è ok, redirect alla details
                return RedirectToAction("Details", new { id = model.IngredientId });
            }
            // se il modello non è valido...
            else
            {
                // ... ritorno la medesima view con i campi precompilati del model ricevuto
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
