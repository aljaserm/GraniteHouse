using GraniteHouse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraniteHouse.ViewModel
{
    public class ShoppingCartVM
    {
        public List<Products> lstProducts { get; set; }
        public Appointments appointments { get; set; }
    }
}
