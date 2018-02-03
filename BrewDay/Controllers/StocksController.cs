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
    public class StocksController : Controller
    {
        private readonly BrewDayContext db = new BrewDayContext();

        // GET: Stocks
        public ActionResult Index()
        {
            var stocks = db.Stocks.Include(s => s.Ingredient);
            return View(stocks.ToList());
        }

        // GET: Stocks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                throw new MissingIdBrewDayException();

            Stock stock = db.Stocks.Find(id);
            if (stock == null)
                throw new InvalidIdBrewDayException(id.Value);

            return View(stock);
        }

        // GET: Stocks/Create
        public ActionResult Create(int? ingredientId)
        {
            ViewBag.Ingredients = new SelectList(db.Ingredients, "IngredientId", "FullName", ingredientId);
            ViewBag.IngReadonly = ingredientId.HasValue ;
            return View();
        }

        // POST: Stocks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Stock stock)
        {
            if (ModelState.IsValid)
            {
                var duplicate = db.Stocks.Where(x => x.ExpireDate == stock.ExpireDate && x.IngredientId == stock.IngredientId).FirstOrDefault();
                if (duplicate != null)
                {
                    duplicate.Quantity = duplicate.Quantity + stock.Quantity;
                    db.Entry(duplicate).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                db.Stocks.Add(stock);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Ingredients = new SelectList(db.Ingredients, "IngredientId", "FullName", stock.IngredientId);
            return View(stock);
        }

        // GET: Stocks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                throw new MissingIdBrewDayException();

            Stock stock = db.Stocks.Find(id);
            if (stock == null)
                throw new InvalidIdBrewDayException(id.Value);

            ViewBag.IngredientId = new SelectList(db.Ingredients, "IngredientId", "Name", stock.IngredientId);
            return View(stock);
        }

        // POST: Stocks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Stock stock)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IngredientId = new SelectList(db.Ingredients, "IngredientId", "Name", stock.IngredientId);
            return View(stock);
        }


        // POST: Stocks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                throw new MissingIdBrewDayException();

            Stock stock = db.Stocks.Find(id);

            if (stock == null)
                throw new InvalidIdBrewDayException(id.Value);

            // deletes element from the context (marks it as "deleted")
            db.Stocks.Remove(stock);

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
