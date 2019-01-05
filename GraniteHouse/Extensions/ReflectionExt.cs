using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraniteHouse.Extensions
{
    public static class ReflectionExt
    {
        public static string GetProperyValue<T>(this T item,string PropertyName)
        {
            return item.GetType().GetProperty(PropertyName).GetValue(item, null).ToString();
        }
    }
}
