using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SBIReportUtility.Common
{
    public static class EnumExtensions
    {
        public static SelectList GetSelectListForEnum<T>(this T enumValue) where T : struct
        {
            IEnumerable<SelectListItem> values = GetEnumValues<T>(enumValue);
            return new SelectList(values, "Value", "Text");
        }

        public static SelectList GetSelectListForEnum<T>(this T enumValue, object selectedItem) where T : struct
        {
            IEnumerable<SelectListItem> values = GetEnumValues<T>(enumValue);
            return new SelectList(values, "Value", "Text", selectedItem);
        }

        private static IEnumerable<SelectListItem> GetEnumValues<T>(T enumValue) where T : struct
        {
            return from Enum e in Enum.GetValues(enumValue.GetType())
                   select new SelectListItem
                     {
                         Value = ((int)Convert.ChangeType(e, typeof(T))).ToString(),
                         Text = e.ToString()
                     };
        }
    }
}
