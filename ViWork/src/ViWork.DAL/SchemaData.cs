using System;
using System.Collections.Generic;
using System.Text;

namespace ViWork.DAL
{
    public class SchemaData
    {
        public int SchemaId { get;  }
        public string SchemaName { get; set; }
        public int GroupID { get; }
        public string GroupName { get; }
    }
}
