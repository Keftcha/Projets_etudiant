using System;
using System.Collections.Generic;
using System.Text;

namespace inProjects.EmailJury
{
    public class EmailerOptions
    {
        public string address {get; set;}
        public string username {get; set;}
        public string password {get; set;}
        public string smtp_server {get; set;}
        public int smtp_server_port {get; set;}
    }
}
