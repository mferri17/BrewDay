using System;
using System.Net;
using System.Linq;
using System.Data.Entity;
using System.Web.Mvc;
using System.Collections.Generic;

using BrewDay.Domain;
using BrewDay.Domain.Entities;

namespace BrewDay.Models.Enums
{
    public class RecipesController : Controller
    {
        private BrewDayContext db = new BrewDayContext();

        // GET: Recipes
        public ActionResult Index()
        {
            var model = db.Recipes.ToList().OrderBy(x => x.FullName).ToList();
            return View(model);
        }

  

        public ActionResult AddIngredient(int id)
        {
            ViewBag.RecipeId = id;
            ViewBag.Ingredients = new SelectList(db.Ingredients, "IngredientId", "FullName");

            return View();
        }

        // POST: Recipes/AddIngredient
        [HttpPost]
        public ActionResult AddIngredient(RecipeIngredient model)
        {
            if (ModelState.IsValid)
            {
                db.RecipeIngredients.Add(model);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = model.RecipeId });
            }
            else
            {
                ViewBag.RecipeId = model.RecipeId;
                ViewBag.IngredientId = new SelectList(db.Ingredients, "IngredientId", "FullName");

                return View(model);
            }
        }

        // GET: Recipes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                throw new MissingIdBrewDayException();

            Recipe recipe = db.Recipes.Find(id);
            if (recipe == null)
                throw new InvalidIdBrewDayException(id.Value);

            return View(recipe);
        }

        // GET: Recipes/Create
        public ActionResult Create()
        {
            var allowedParents = db.Recipes.Where(x => !x.ParentRecipeId.HasValue);
            ViewBag.ParentRecipeId = new SelectList(allowedParents, "RecipeId", "Name");

            return View();
        }

        // POST: Recipes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                db.Recipes.Add(recipe);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var allowedParents = db.Recipes.Where(x => !x.ParentRecipeId.HasValue);
            ViewBag.ParentRecipeId = new SelectList(allowedParents, "RecipeId", "Name");

            return View(recipe);
        }

        // GET: Recipes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                throw new MissingIdBrewDayException();

            Recipe recipe = db.Recipes.Find(id);
            if (recipe == null)
                throw new InvalidIdBrewDayException(id.Value);

            var allowedParents = db.Recipes.Where(x => x.RecipeId != id && !x.ParentRecipeId.HasValue);
            ViewBag.ParentRecipeId = new SelectList(allowedParents, "RecipeId", "Name", recipe.ParentRecipeId);

            return View(recipe);
        }

        // POST: Recipes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recipe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var allowedParents = db.Recipes.Where(x => x.RecipeId != recipe.RecipeId && !x.ParentRecipeId.HasValue);
            ViewBag.ParentRecipeId = new SelectList(allowedParents, "RecipeId", "Name", recipe.ParentRecipeId);

            return View(recipe);
        }

        // GET: Recipes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                throw new MissingIdBrewDayException();

            Recipe recipe = db.Recipes.Find(id);
            if (recipe == null)
                throw new InvalidIdBrewDayException(id.Value);

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Recipe recipe = db.Recipes.Find(id);
                db.Recipes.Remove(recipe);
                // OCIO! Bisogna modificare il database : se si tenta di eliminare una ricetta padre senza aver cancellato le figlie si solleva un'eccezione!
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                throw new InvalidOperationBrewDayException("Non è possibile cancellare una Ricetta che possiede versioni alternative della stessa. Cancellare prima le versioni.");
            }
        }


        // POST : Recipes/RemoveIngredient?recipedId & ingredientId
        public ActionResult RemoveIngredient(int recipeId, int ingredientId)
        {
            Recipe recipe = db.Recipes.Find(recipeId);
            RecipeIngredient recipeIngredient = db.RecipeIngredients.Find(recipeId, ingredientId);
            recipe.Ingredients.Remove(recipeIngredient);
            db.Entry(recipe).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", new { id = recipeId });
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
