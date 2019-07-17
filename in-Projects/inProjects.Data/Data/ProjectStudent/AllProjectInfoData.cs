using inProjects.Data.Data.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace inProjects.Data.Data.ProjectStudent
{
    public class AllProjectInfoData
    {
        public int ProjectStudentId { get; set; }
        public string Logo { get; set; }
        public string Background { get; set; }
        public string Slogan { get; set; }
        public string Pitch { get; set; }
        public int LeaderId { get; set; }
        public string Type { get; set; }
        public int TraitId { get; set; }
        public string Semester { get; set; }
        public string TraitName { get; set; }
        public string GroupName { get; set; }
        public int ZoneId { get; set; }
        public int SchoolId { get; set; }
        public List<string> Technologies { get; set; }
        public string ClassRoom { get; set; }
        public string Sector { get; set; }
        public int ForumNumber { get; set; }
        public int JuryId { get; set; }


        public List<string> FirstName = new List<string>();
        public List<string> LastName = new List<string>();
        public List<int> TimedUserId = new List<int>();
        public List<int> UserId = new List<int>();
        public DateTime BegDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<UserData> UsersData { get; set; }

        public int IsFav { get; set; }

        public double Grade { get; set; }

        public bool IsBlocked { get; set; }

    }
}
