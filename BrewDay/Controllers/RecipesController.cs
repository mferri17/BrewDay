﻿using System;
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
        private readonly BrewDayContext db = new BrewDayContext();

        // GET: Recipes
        public ActionResult Index()
        {
            var model = db.Recipes.ToList();
            return View(model);
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
                return RedirectToAction("Details", new { id = recipe.RecipeId });
            }

            var allowedParents = db.Recipes.Where(x => x.RecipeId != recipe.RecipeId && !x.ParentRecipeId.HasValue);
            ViewBag.ParentRecipeId = new SelectList(allowedParents, "RecipeId", "Name", recipe.ParentRecipeId);

            return View(recipe);
        }



        // POST: Recipes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                throw new MissingIdBrewDayException();

            Recipe recipe = db.Recipes.Find(id);
            if (recipe == null)
                throw new InvalidIdBrewDayException(id.Value);

            if (recipe.Productions.Count > 0 )
                throw new InvalidOperationBrewDayException("Non è possibile cancellare una Ricetta che possiede delle Produzioni (in corso o non). Cancellare prima le Produzioni.");
            if (recipe.Versions.Count > 0)
                throw new InvalidOperationBrewDayException("Non è possibile cancellare una Ricetta che possiede versioni alternative della stessa. Cancellare prima le Versioni.");

            // deletes element from the context (marks it as "deleted")
            db.Recipes.Remove(recipe);

            // takes changes from context and makes it real on the database
            db.SaveChanges();

            // if everything's ok, redirect to the index
            return RedirectToAction("Index");
        }

        public ActionResult Clone(int? id, string name)
        {
            if (id == null)
                throw new MissingIdBrewDayException();

            Recipe recipe = db.Recipes.Find(id);
            if (recipe == null)
                throw new InvalidIdBrewDayException(id.Value);

            Recipe newRecipe = new Recipe()
            {
                Name = !string.IsNullOrEmpty(name) ? name : recipe.Name,
                Description = recipe.Description,
                Note = recipe.Note,
                FermentationTemperature = recipe.FermentationTemperature,
                ParentRecipeId = recipe.RecipeId,
                FermentationTime = recipe.FermentationTime,
                Productions = null,
            };
            db.Recipes.Add(newRecipe);
            db.SaveChanges();

            var newRecipeIngredients = recipe.Ingredients.Select(x => new RecipeIngredient() { RecipeId = newRecipe.RecipeId.Value, IngredientId = x.IngredientId, Quantity = x.Quantity });
            newRecipe.Ingredients = new List<RecipeIngredient>(newRecipeIngredients);
            db.Entry(newRecipe).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Edit", new { id = newRecipe.RecipeId });
        }



        public ActionResult AddIngredient(int? id)
        {
            if (!id.HasValue)
                throw new MissingIdBrewDayException();

            var recipe = db.Recipes.Find(id.Value);
            if (recipe == null)
                throw new InvalidIdBrewDayException(id.Value);
            if (recipe.HasProductions)
                throw new InvalidOperationBrewDayException("Non è possibile aggiungere Ingredienti a una Ricetta che già possiede Produzioni.");

            ViewBag.RecipeId = id;
            ViewBag.RecipeFullName = recipe.FullName;
            ViewBag.Ingredients = new SelectList(db.Ingredients, "IngredientId", "FullName");

            return View();
        }

        // POST: Recipes/AddIngredient
        [HttpPost]
        public ActionResult AddIngredient(RecipeIngredient model)
        {
            if (ModelState.IsValid)
            {
                var recipe = db.Recipes.Find(model.RecipeId);
                if (recipe.HasProductions)
                    throw new InvalidOperationBrewDayException("Non è possibile aggiungere Ingredienti a una Ricetta che già possiede Produzioni.");

                var duplicated = db.RecipeIngredients.Find(model.RecipeId, model.IngredientId);

                if (duplicated == null)
                {
                    db.RecipeIngredients.Add(model);
                }
                else
                {
                    duplicated.Quantity += model.Quantity;
                    db.Entry(duplicated).State = EntityState.Modified;
                }
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
