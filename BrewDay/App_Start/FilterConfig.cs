using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Linq;

namespace BrewDay
{
    public static class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute()); //default handler
            filters.Add(new HandleExceptionsAttribute());
        }
    }

    /// <summary>
    /// Custom HandleErrorAttribute to manage Exceptions, redirecting to a specific View with error messages.
    /// </summary>
    public class HandleExceptionsAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            Exception exception = filterContext.Exception;
            List<string> messages = new List<string>();
            
            if (!(exception is BrewDayException))
                messages.Add("Si è verificato un errore interno del server. Contattare l'amministratore per segnalare il bug.");

            filterContext.Result = new ViewResult()
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary {
                    { "Messages", messages.Concat(GetAllMessages(exception)) },
                    { "StackTrace", exception.StackTrace }
                }
            };
            filterContext.ExceptionHandled = true;
        }

        /// <summary>
        /// Given an Exception, returns all Messages taken from InnerExceptions.
        /// </summary>
        private List<string> GetAllMessages(Exception exception)
        {
            List<string> result = new List<string>();

            while (exception != null)
            {
                result.Add(exception.Message);
                exception = exception.InnerException;
            }

            return result;
        }
    }
}
