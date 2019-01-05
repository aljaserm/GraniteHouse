using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraniteHouse.Extensions
{
    public static class IEnumerableExt
    {
        public static IEnumerable<SelectListItem> selectListItems<T>(this IEnumerable<T> items,int selectedVal)
        {
            return from item in items
                   select new SelectListItem
                   {
                       Text = item.GetProperyValue("Name"),
                       Value = item.GetProperyValue("Id"),
                       Selected = item.GetProperyValue("Id").Equals(selectedVal.ToString())
                   };
        }
    }
}
