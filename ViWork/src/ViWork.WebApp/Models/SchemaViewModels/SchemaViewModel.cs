using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViWork.WebApp.Models.SchemaViewModels
{
    public class SchemaViewModel
    {
        public int SchemaId { get; }
        public string SchemaName { get; set; }
        public int GroupId { get; set; }
    }
}
