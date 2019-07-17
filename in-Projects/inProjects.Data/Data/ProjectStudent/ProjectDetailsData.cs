using inProjects.Data.Data.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace inProjects.Data.Data.ProjectStudent
{
   public class ProjectDetailsData
    {
        public ProjectData Project { get; set; }
        public List<UserData> Students { get; set; }
    }
}
