using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repository
{
    public class JsonFormatRepository : IJsonFormatRepository
    {
        private IConfiguration _configuration;

        public JsonFormatRepository( IConfiguration configuration )
        {
            _configuration = configuration;
        }


        public Output GetJson_NMC(DateTime fromDate, DateTime toDate ,string txn_id, List<PatientDetail> opdPatientList, List<PatientDetail> ipdPatientList , string hf_id_hmis)
        {

            Output output = new Output();
            output.result = new List<Result>();

            string version = _configuration.GetValue<string>("HospitalSetting:version");

            Metadata metadata = new Metadata() { code = "200", message = "transaction successful", timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss",CultureInfo.InvariantCulture),version= version };

            //  List<Result> result = new List<Result>();

            Result result = new Result();

            result.module_wise_kpi = new List<ModuleWiseKpi>();


            result.txn_id =  txn_id;
            result.from_date = fromDate.ToString("dd/MM/yyyy HH:mm:ss" ,CultureInfo.InvariantCulture);
            result.to_date = toDate.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            result.hf_id_hmis = _configuration.GetValue<string>("HospitalSetting:hmis_code"); 
            result.hf_id_abdm = _configuration.GetValue<string>("HospitalSetting:ABDM_FacilityId");
            result.health_facility_name = _configuration.GetValue<string>("HospitalSetting:FacilityName");

            ModuleWiseKpi module_wise_kpi_OPD = new ModuleWiseKpi();


            //for opd patient
            module_wise_kpi_OPD.module_code = "01";
            module_wise_kpi_OPD.module_name = "OPD";
            module_wise_kpi_OPD.hmis_code = _configuration.GetValue<string>("HospitalSetting:hmis_code");
            module_wise_kpi_OPD.opd_count = opdPatientList.Count;

            if (module_wise_kpi_OPD.ipd_count==null)
            {
                module_wise_kpi_OPD.ipd_count = 0;
            }

            // Initialize opd list
            module_wise_kpi_OPD.patient_details = new List<PatientDetail>();


            //Assign opd list
            module_wise_kpi_OPD.patient_details = opdPatientList;


            // for IPD patient
            ModuleWiseKpi module_wise_kpi_IPD = new ModuleWiseKpi();

            module_wise_kpi_IPD.module_code = "02";
            module_wise_kpi_IPD.module_name = "IPD";
            module_wise_kpi_IPD.hmis_code = _configuration.GetValue<string>("HospitalSetting:hmis_code");
            module_wise_kpi_IPD.ipd_count = ipdPatientList.Count();

            if (module_wise_kpi_IPD.opd_count == null)
            {
                module_wise_kpi_IPD.opd_count = 0;
            }

            //Initialize ipd list
            module_wise_kpi_IPD.patient_details = new List<PatientDetail>();
           




            //Assign ipd list
            module_wise_kpi_IPD.patient_details = ipdPatientList;




            //  Root obj = new Root();
            //  obj.Output = output;



            result.module_wise_kpi.Add(module_wise_kpi_OPD);
            result.module_wise_kpi.Add(module_wise_kpi_IPD);



            // Assign Meta data
            output.metadata = metadata;


           // validation as per NMC

           if (result.module_wise_kpi.Count<=0)
            {
                output.metadata.code =StatusCodes.Status204NoContent.ToString();
                output.metadata.message= "data not available for the requested duration";
            }


            // Add opd ,ipd result

            output.result.Add(result);



            // return JSON_NMC
            return output;
        }
    }
}
