using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BrewDay.Models;
using BrewDay.Models.Entities;

namespace BrewDay.Controllers
{
    public class ProductionsController : Controller
    {
        private BrewDayContext db = new BrewDayContext();

        // GET: Productions
        public ActionResult Index()
        {
            return View(db.Production.ToList());
        }

        // GET: Productions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Production production = db.Production.Find(id);
            if (production == null)
            {
                return HttpNotFound();
            }
            return View(production);
        }

        // GET: Productions/Create
        public ActionResult Create()
        {
            return View();
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
                db.Production.Add(production);
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
            Production production = db.Production.Find(id);
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
            Production production = db.Production.Find(id);
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
            Production production = db.Production.Find(id);
            db.Production.Remove(production);
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
