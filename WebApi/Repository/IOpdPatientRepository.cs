using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repository
{
   public interface IOpdPatientRepository
    {

      List<PatientDetail> GetAllOPDPatients(DateTime fromDate,DateTime toDate);
    }
}
