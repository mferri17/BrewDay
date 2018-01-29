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
            var productions = db.Productions.Where(x => x.DateEndEstimated > DateTime.Now).ToList(); // finiscono entro una settimana o sono già finite (data stimata di fine)
            var stocks = db.Stocks.Where(x => true).ToList(); // quelli che stanno scadendo (entro settimana) o sotto una certa quantità (10?)

            ProductsAudit model = new ProductsAudit() {
                FinishingProduction = productions,
                RunningOutStocks = stocks
            };

            return View(model);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}