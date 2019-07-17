using System;
using System.Collections.Generic;
using System.Text;

namespace inProjects.Data.Data.ProjectStudent
{
    public class ProjectInfosJuryData
    {
        public int ProjectStudentId { get; set; }
        public int JuryId { get; set; }
        public string ProjectName { get; set; }
        public string JuryName { get; set; }
        public double Grade { get; set; }
        public bool IsBlocked { get; set; }
        public string ClassRoom { get; set; }
        public int ForumNumber { get; set; }
    }
}
