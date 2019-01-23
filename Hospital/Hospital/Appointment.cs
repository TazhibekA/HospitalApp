using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital
{
    public class Appointment
    {
        public int Id { get; set; }
     
        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }
        public string ClientFullName { get; set; } 
        public DateTime Time { get; set; }

    }
}
