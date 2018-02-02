using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrewDay.Domain.Enums
{
    public static class EnumHelper
    {
        /// <summary>
        /// Returns enum value as shown in display name data annotation
        /// </summary>
        /// <param name="item">Directly invoke from an Enum property</param>
        /// <returns></returns>
        public static string DisplayEnum(this Enum item)
        {
            var type = item.GetType();
            var member = type.GetMember(item.ToString());
            DisplayAttribute displayname = (DisplayAttribute)member[0].GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();

            if (displayname != null)
            {
                return displayname.Name;
            }

            return item.ToString();
        }
    }
}
