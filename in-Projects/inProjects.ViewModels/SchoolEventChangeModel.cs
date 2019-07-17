using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace inProjects.ViewModels
{
   public class SchoolEventChangeModel
    {
        [Required]
        public int EventId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime BegDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        //If name is "Other" set variable to true
        public bool IsOther { get; set; }
    }
}
