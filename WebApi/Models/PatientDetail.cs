using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class PatientDetail
    {

        public string patient_name { get; set; }
        public string visit_id { get; set; }
        public string year_of_birth { get; set; }

        public string address { get; set; }
        public string patient_abha_id { get; set; }
        public string patient_identification_proof { get; set; }
        public string patient_identification_number { get; set; }
        public string patient_mobile_number { get; set; }
        public string transaction_type { get; set; }
        public string uhid_number { get; set; }
        public string admission_number { get; set; }
        public string department_visited_name { get; set; }
        public string department_visited_code { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd/MM/yyyy HH:mm:ss}")]
        public string datetime_of_transaction { get; set; }
    
      //  public string department_admitted_name { get; set; }
    }
}
