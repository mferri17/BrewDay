using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;


namespace BrewDay
{
    public class FilterConfig
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
            Exception ex = filterContext.Exception;

            filterContext.Result = new ViewResult()
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary {
                    { "Messages", GetAllMessages(ex) },
                    { "StackTrace", ex.StackTrace }
                }
            };
            filterContext.ExceptionHandled = true;
        }

        /// <summary>
        /// Given an Exception, returns all Messages taken from InnerExceptions.
        /// </summary>
        private string[] GetAllMessages(Exception exception)
        {
            List<string> result = new List<string>();

            while (exception != null)
            {
                result.Add(exception.Message);
                exception = exception.InnerException;
            }

            return result.ToArray();
        }
    }
}
