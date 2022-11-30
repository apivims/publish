using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Result
    {

        public string txn_id { get; set; }
        public string from_date { get; set; }
        public string to_date { get; set; }
        public string hf_id_hmis { get; set; }
        public string hf_id_abdm { get; set; }
        public string health_facility_name { get; set; }
        public List<ModuleWiseKpi> module_wise_kpi { get; set; }
    }
}
