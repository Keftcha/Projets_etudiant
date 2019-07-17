using System;
using System.Collections.Generic;
using System.Text;

namespace inProjects.Data.Data.TimePeriod
{
   public class TimePeriodData
    {
        public int TimePeriodId { get; set; }
        public DateTime BegDate { get; set; }
        public DateTime EndDate { get; set; }
        public char Kind { get; set; }
    }
}
