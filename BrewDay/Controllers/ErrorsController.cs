using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BrewDay.Controllers
{
    public class ErrorsController : Controller
    {
        public ActionResult PageNotFound()
        {
            Response.StatusCode = 404;
            ViewBag.Messages = new string[] { "Controlla che l'URL o l'Id della risorsa richiesta sia corretto." };
            return View("Error");
        }

        public ActionResult BadRequest()
        {
            Response.StatusCode = 400;
            ViewBag.Messages = new string[] { "Qualcosa è andato storto ma non è possibile avere informazioni più dettagliate." };
            return View("Error");
        }
    }
}