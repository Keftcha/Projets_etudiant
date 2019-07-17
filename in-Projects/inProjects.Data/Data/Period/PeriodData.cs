

using System;

namespace inProjects.Data.Data.Period
{
    public class PeriodData
    {
        public string OrderedChil { get; set; }
        public int ChildId { get; set; }

        public int ZoneId { get; set; }

        public DateTime BegDate { get; set; }
        public DateTime EndDate { get; set; }

        public string GroupName { get; set; }
    }
}
