using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repository
{
   public interface IIPdPatientRepository
    {

      List<PatientDetail> GetAllIPDPatients(DateTime fromDate,DateTime toDate);
    }
}
