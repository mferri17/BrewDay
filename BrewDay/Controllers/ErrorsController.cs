using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BrewDay.Controllers
{
    public class ErrorsController : Controller
    {
        public ActionResult BadRequest()
        {
            Response.StatusCode = 400;
            ViewBag.Messages = new string[] { "Qualcosa è andato storto ma non è possibile avere informazioni più dettagliate. Assicurati di aver provato ad eseguire un'operazione fattibile." };
            return View("Error");
        }


        public ActionResult Forbidden()
        {
            Response.StatusCode = 403;
            ViewBag.Messages = new string[] { "Non hai l'autorizzazione per accedere a questa risorsa." };
            return View("Error");
        }

        public ActionResult PageNotFound()
        {
            Response.StatusCode = 404;
            ViewBag.Messages = new string[] { "Controlla che l'URL o l'Id della risorsa richiesta sia corretto." };
            return View("Error");
        }

        public ActionResult InternalServerError()
        {
            Response.StatusCode = 500;
            ViewBag.Messages = new string[] { "Qualcosa sul server è andato storto, purtroppo non è possibile avere informazioni più dettagliate." };
            return View("Error");
        }
    }
}