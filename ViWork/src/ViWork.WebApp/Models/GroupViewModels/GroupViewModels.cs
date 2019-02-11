using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViWork.WebApp.Models.GroupViewModels
{
    public class GroupViewModels
    {
        public int GroupId { get; }
        public string GroupName { get; set; }
        public int OwnerID { get; set; }
    }
}
