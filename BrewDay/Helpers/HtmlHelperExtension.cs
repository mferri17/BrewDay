using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using BrewDay.Domain.Enums;

namespace System.Web.Mvc.Html
{
    public static class HtmlHelperExtension
    {
        /// <summary>
        /// Returns enum value as shown in display name data annotation
        /// </summary>
        /// <param name="item">Enum object value to be displayed</param>
        /// <returns></returns>
        public static IHtmlString DisplayEnumLabel(this HtmlHelper html, Enum item)
        {
            return new MvcHtmlString(item.DisplayEnum());
        }
    }
}