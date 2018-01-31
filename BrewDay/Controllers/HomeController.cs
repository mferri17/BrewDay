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
            return View(new ProductsAudit());
        }

        public ActionResult About()
        {
            return View();
        }
    }
}