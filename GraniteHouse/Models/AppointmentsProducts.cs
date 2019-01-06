using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GraniteHouse.Models
{
    public class AppointmentsProducts
    {
        public int ID { get; set; }
        
        public int appointmentId { get; set; }
        [ForeignKey("appointmentId")]
        public virtual Appointments appointments { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Products products { get; set; }
    }
}
