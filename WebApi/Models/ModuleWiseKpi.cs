using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class ModuleWiseKpi
    {
        public string module_code { get; set; }
        public string module_name { get; set; }
        public string hmis_code { get; set; }
        public int? opd_count { get; set; }
        public int? ipd_count { get; set; }
        public List<PatientDetail> patient_details { get; set; }
       

    }
}
