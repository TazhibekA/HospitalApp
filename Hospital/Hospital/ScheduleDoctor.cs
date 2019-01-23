using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital
{
    public class ScheduleDoctor
    {
        public int Id { get; set; }
        
        public TimeSpan StartTime { get; set; }
        public TimeSpan FinishTime { get; set; }
        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }

        //Console.WriteLine("t: {0:t}", now);
    }
}
