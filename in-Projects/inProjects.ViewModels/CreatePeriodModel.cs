using System;
using System.Collections.Generic;
using System.Text;

namespace inProjects.ViewModels
{
    public class CreatePeriodModel
    {
        public DateTime begDate { get; set; }
        public DateTime endDate { get; set; }
        public string Kind { get; set; }
        public List<GroupPeriod> Groups { get; set; }
        public int idZone { get; set; }
    }
}
