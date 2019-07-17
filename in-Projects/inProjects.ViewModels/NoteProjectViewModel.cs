using System;
using System.Collections.Generic;
using System.Text;

namespace inProjects.ViewModels
{
    public class NoteProjectViewModel
    {
        public int ProjectId {get; set;}
        public double Grade {get; set;}
        public int SchoolId { get; set; }
        public int JuryId { get; set; }
        public TypeTimedUser User { get; set; }
    }

    public enum TypeTimedUser
    {
        Anon,
        Student,
        StaffMember,
        Jury,

    }
}
