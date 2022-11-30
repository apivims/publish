using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repository
{
   public interface IJsonFormatRepository
    {
        

      public  Output GetJson_NMC(DateTime fromDate, DateTime toDate, string txn_id,
            List<PatientDetail> opdPatientList, List<PatientDetail> ipdPatientList ,string hf_id_hmis);
    }
}
