using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

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
            var type = item.GetType();
            var member = type.GetMember(item.ToString());
            DisplayAttribute displayname = (DisplayAttribute)member[0].GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();

            if (displayname != null)
            {
                return new MvcHtmlString(displayname.Name);
            }

            return new MvcHtmlString(item.ToString());
        }
    }
}