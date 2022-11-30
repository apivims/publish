using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Metadata
    {
        public string code { get; set; }
        public string message { get; set; }
        public string timestamp { get; set; }
        public string version { get; set; }
    }
}
