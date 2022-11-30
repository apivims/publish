using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Output
    {
        public Metadata metadata { get; set; }
      //  public Result result { get; set; }
        public List<Result> result { get; set; }
    }
}
