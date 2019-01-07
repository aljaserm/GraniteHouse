using GraniteHouse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraniteHouse.ViewModel
{
    public class AppointmentsDetailsVM
    {
        public Appointments appointments { get; set; }
        public List<Users> lstUsers { get; set; }
        public List<Products> lstproducts { get; set; }
    }
}
