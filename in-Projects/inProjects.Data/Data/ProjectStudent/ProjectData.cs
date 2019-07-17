using System;
using System.Collections.Generic;
using System.Text;

namespace inProjects.Data.Data.ProjectStudent
{
    public class ProjectData
    {
        public int ProjectStudentId { get; set; }
        public string Logo { get; set; }
        public string Background { get; set; }
        public string Slogan { get; set; }
        public string Pitch { get; set; }
        public int LeaderId { get; set; }
        public string Type { get; set; }
        public int TraitId { get; set; }
        public string Name { get; set; }
        public string Semester { get; set; }
        public List<string> Technologies { get; set; }
        public string Url { get; set; }
        public int CoordinatesX { get; set; }
        public int CoordinatesY { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string ClassRoom { get; set; }
        public int ForumNumber { get; set; }
        

        public string TraitName { get; set; }
        public string GroupName { get; set; }
        public int ZoneId { get; set; }
    }
}
