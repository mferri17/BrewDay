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
    public class InstrumentsController : Controller
    {
        private readonly BrewDayContext db = new BrewDayContext();

        // GET: Instruments
        public ActionResult Index()
        {
            return View(db.Instruments.ToList());
        }

        // GET: Instruments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                throw new MissingIdBrewDayException();

            Instrument instrument = db.Instruments.Find(id);
            if (instrument == null)
                throw new InvalidIdBrewDayException(id.Value);

            return View(instrument);
        }

        // GET: Instruments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Instruments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Instrument instrument)
        {
            if (ModelState.IsValid)
            {
                db.Instruments.Add(instrument);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(instrument);
        }

        // GET: Instruments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                throw new MissingIdBrewDayException();

            Instrument instrument = db.Instruments.Find(id);
            if (instrument == null)
                throw new InvalidIdBrewDayException(id.Value);

            return View(instrument);
        }

        // POST: Instruments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Instrument instrument)
        {
            if (ModelState.IsValid)
            {
                if (instrument.Used > instrument.Quantity)
                    throw new InvalidOperationBrewDayException("Non puoi inserire una quantità si Strumenti inferiore a quella attualmente in uso nelle Produzioni in corso.");
                db.Entry(instrument).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(instrument);
        }

        // POST: Instruments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                throw new MissingIdBrewDayException();

            Instrument instrument = db.Instruments.Find(id);

            if (instrument == null)
                throw new InvalidIdBrewDayException(id.Value);

            if (instrument.Used > 0)
                throw new InvalidOperationBrewDayException("Non puoi eliminare uno strumento ancora impegnato in una produzione!");

            // deletes element from the context (marks it as "deleted")
            db.Instruments.Remove(instrument);

            // takes changes from context and makes it real on the database
            db.SaveChanges();

            // if everything's ok, redirect to the index
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}
