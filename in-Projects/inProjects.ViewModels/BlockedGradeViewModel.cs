using System;
using System.Collections.Generic;
using System.Text;

namespace inProjects.ViewModels
{
    public class BlockedGradeViewModel
    {
        public int ProjectId { get; set; }
        public List<int> JurysId { get; set; }
        public Dictionary<string, double> IndividualGrade { get; set; }
    }
}
