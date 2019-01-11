using GraniteHouse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraniteHouse.ViewModel
{
    public class AppointmentsVM
    {
        public List<Appointments> appointments {get;set;}
        public PagingInfo PagingInfo { get; set; }
    }
}
