using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WebApi.Error;
using WebApi.Model_hims;
using WebApi.Models;
using WebApi.Repository;

namespace WebApi.Controllers
{

    [Authorize]
    [Route("[controller]/[action]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        //public string GetAllPatients()
        //{
        //    return "hello from get all";
        //}
        private readonly IConfiguration _configuration;
        private readonly IOpdPatientRepository _opdPatientRepository;
        private readonly IIPdPatientRepository _ipdPatientRepository;
        private readonly IJsonFormatRepository _jsonFormatRepository;
        private readonly IRequestHeaderRepository _requestHeaderRepository;
        private ILog _logger;

        public PatientsController(IConfiguration iconfiguration, IOpdPatientRepository opdPatientRepository  ,IIPdPatientRepository iPdPatientRepository ,
                   IJsonFormatRepository jsonFormatRepository ,IRequestHeaderRepository requestHeaderRepository ,ILog logger )
         {
            _configuration = iconfiguration;
            _opdPatientRepository = opdPatientRepository;
            _ipdPatientRepository = iPdPatientRepository;
            _jsonFormatRepository = jsonFormatRepository;
            _requestHeaderRepository = requestHeaderRepository;
            _logger = logger;



         }
       
        [HttpGet]
        [Authorize]
        public  IActionResult GetAllPatients([FromHeader] string hfidABDM, [FromHeader] string hfidHMIS,
                      [FromHeader] string fromDate,[FromHeader] string toDate, [FromHeader] string syncRequestType,
                       [FromHeader] string txn_id, [FromHeader] string module_code   )

            {


            string _hfidABDM;
            string _hfidHMIS;
            string _fromDate;
            string _toDate;
        
            string _syncRequestType;
            string _txn_id;
            string _module_code;
            Dictionary<string, string> dicHeader = new Dictionary<string, string>();
            //Dictionary variable 
            RequestHeader requestHeader = new RequestHeader();
            string reqReult ="0";

            List<PatientDetail> opdPatientList =new List<PatientDetail>();

            List<PatientDetail> ipdPatientList =new List<PatientDetail>();

            DateTime mfromDate =DateTime.Now;
            DateTime mtoDate= DateTime.Now;

          


            try
            {
               



                //  Dictionary<string, IEnumerable<string>> ss = Request.Headers.ToDictionary(a => a.Key, a => a.Value);

                //foreach (var header in Request.Headers)
                //{
                //    dicHeader.Add(header.Key, header.Value);
                //}


                //Declare local Variable/parameters

              


                //Assign requested value to local variables/parameters

                _hfidABDM = hfidABDM;
                _hfidHMIS = hfidHMIS;
                _fromDate = fromDate;
                _toDate = toDate;
               
                _syncRequestType = syncRequestType;
                _txn_id = txn_id;
                _module_code = module_code;

                // Request header section

                if (_txn_id == null)
                {
                    _txn_id = "";
                }

                if (_module_code == null)
                {
                    _module_code = "";
                }
               


                if (_syncRequestType == null)
                {
                    _syncRequestType = "";
                }

                if (_hfidABDM == null)
                {
                    _hfidABDM = "";
                }

                if (_hfidHMIS == null)
                {
                    _hfidHMIS = "";
                }

               



                requestHeader.hfidABDM = _hfidABDM;
                requestHeader.hfidHMIS = _hfidHMIS;
                requestHeader.fromDate = _fromDate;
                requestHeader.toDate = _toDate;
             
                requestHeader.syncRequestType = _syncRequestType;

              


                requestHeader.txn_id = _txn_id;

                requestHeader.module_code = module_code;
                requestHeader.msg = "";
                requestHeader.Authorization = "";




               // reqReult = _requestHeaderRepository.AddRequestHeader(requestHeader);
                // Patient section

             

                 mfromDate = DateTime.ParseExact(_fromDate, "dd/MM/yyyy HH:mm:ss",
                                               System.Globalization.CultureInfo.InvariantCulture);
                 mtoDate  = DateTime.ParseExact(_toDate, "dd/MM/yyyy HH:mm:ss",
                                              System.Globalization.CultureInfo.InvariantCulture);

                var reqResult = _requestHeaderRepository.AddRequestHeader(requestHeader);
                // Patient section

                _logger.Information($"---from date: { fromDate.ToString() }-----To {toDate.ToString()}----------------Transaction id{requestHeader.txn_id.ToString() }-----");

                // This is restricted date;below this date data will not fetch
                DateTime validDate;


                validDate = DateTime.ParseExact(_configuration.GetValue<string>("HospitalSetting:LiveDate"), "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);


                // Cast Statement for module code

                if (mtoDate > validDate)
                {

                    switch (_module_code.Trim())
                    {

                        case "01" or "1":
                            // code block
                            opdPatientList = _opdPatientRepository.GetAllOPDPatients(mfromDate, mtoDate);
                            break;
                        case "02" or "2":
                            // code block
                            ipdPatientList = _ipdPatientRepository.GetAllIPDPatients(mfromDate, mtoDate);
                            break;
                        default:
                            // code block
                            opdPatientList = _opdPatientRepository.GetAllOPDPatients(mfromDate, mtoDate);
                            ipdPatientList = _ipdPatientRepository.GetAllIPDPatients(mfromDate, mtoDate);

                            break;

                        
                    }
                 
                }
               
               

                




                //-------------------------------------------------------------------
                //-------- Generate Data Into JSON Format----------------------------
                //-------------------------------------------------------------------

               var json  = _jsonFormatRepository.GetJson_NMC(mfromDate, mtoDate, _txn_id, opdPatientList, ipdPatientList, _hfidHMIS);



            //    var result = JsonConvert.SerializeObject(json,
            //new JsonSerializerSettings()
            //{
            //    NullValueHandling = NullValueHandling.Ignore
            //});



             

                 return Ok(json);

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);

                _logger.Error(ex.StackTrace);
             
                return StatusCode(500,ex.Message);

            }

           
                
                //----------------------------------------------



              


                // return Ok(  JsonConvert.SerializeObject(obj ));


                // return strJson;




        }

        [HttpGet]
        public IActionResult Get3()
        {
            return Ok("Admin");
        }



        public Output GetRawData()
        {


          

            

            Output output = new Output();
            output.result = new List<Result>();
           
            Metadata metadata = new Metadata() { code = "200", message = "transaction successful", timestamp = "02/04/2020 19:12:15" };

          //  List<Result> result = new List<Result>();

            Result result = new Result();

            result.module_wise_kpi = new List<ModuleWiseKpi>();



            result.from_date = "25/05/2022 00:00:00";
            result.to_date = "25/06/2022 23:59:59";
            result.hf_id_hmis = "66";
            result.hf_id_abdm = "IN0710000001";
            result.health_facility_name = "Dr.Vikhe Patil Hospital";
            
            ModuleWiseKpi module_wise_kpi_OPD = new ModuleWiseKpi();


            //for opd patient
            module_wise_kpi_OPD.module_code = "1";
            module_wise_kpi_OPD.module_name = "OPD";
            module_wise_kpi_OPD.hmis_code = "002";
            module_wise_kpi_OPD.opd_count = 3;
           
            module_wise_kpi_OPD.patient_details = new List<PatientDetail>();

            List<PatientDetail> opd_patient_details =new List<PatientDetail>();

            opd_patient_details.Add(new PatientDetail() 
            { patient_name= "Manoj Tiwari", year_of_birth= "2015", address= "At post + Tah -Loni, Pocket - N," +
                 " Sarita Vihar, AhmadNager - 320008", patient_abha_id="" , patient_identification_proof= "Aadhaar Card" ,  patient_identification_number= "302545687895",visit_id= "15688798",
                patient_mobile_number="9850932602", transaction_type="1", uhid_number= "20220000587", department_visited_name="Medicine"  ,department_visited_code="00001" ,  datetime_of_transaction= DateTime.Today.ToString()
            });;


            opd_patient_details.Add(new PatientDetail()
            {
                patient_name = "Vinod Bhandarkar",
                year_of_birth = "2016",
                visit_id = "78999",//visit id
                address = "319 B, Pocket - N," +
               " Sarita Vihar, AhmadNager - 320008",
                patient_abha_id = "",
                patient_identification_proof = "Aadhaar Card",
                patient_identification_number = "302545687894",
                patient_mobile_number = "9850930201",
                transaction_type = "1",
                uhid_number = "20220000582",
                department_visited_name = "Medicine",
                department_visited_code = "00001",
                datetime_of_transaction = DateTime.Today.ToString()
            }) ;



            opd_patient_details.Add(new PatientDetail()
            {
                patient_name = "Rahul Vaidya",
                year_of_birth = "2016",
                visit_id = "569999",//ipd id
                address = "319 B, Pocket - N," +
              " Sarita Vihar, New Delhi - 110000",
                patient_abha_id = "",
                patient_identification_proof = "Aadhaar Card",
                patient_identification_number = "402545687895",
                patient_mobile_number = "9850932602",
                transaction_type = "1",
                uhid_number = "20220000587",
                department_visited_name = "Ortho",
                department_visited_code = "00002",
                datetime_of_transaction = DateTime.Today.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
            }) ;



            module_wise_kpi_OPD.patient_details = opd_patient_details;


            // for IPD patient
            ModuleWiseKpi module_wise_kpi_IPD = new ModuleWiseKpi();

            module_wise_kpi_IPD.module_code = "2";
            module_wise_kpi_IPD.module_name = "IPD";
            module_wise_kpi_IPD.hmis_code = "002";
            module_wise_kpi_IPD.ipd_count = 2;
            module_wise_kpi_IPD.patient_details = new List<PatientDetail>();


            List<PatientDetail> Ipd_patient_details = new List<PatientDetail>();


            Ipd_patient_details.Add(new PatientDetail()
            {
                patient_name = "Harish Bonde",
                year_of_birth = "2001",
                visit_id = "56666",//ipd id
                address = "pune -411038",
                patient_abha_id = "",
                patient_identification_proof = "Aadhaar Card",
                patient_identification_number = "402545687895",
                patient_mobile_number = "9850932602",
                transaction_type = "",
                uhid_number = "20220000587",
                admission_number = "1540068",
               // department_admitted_name = "Medicine",

                department_visited_code = "00002",
                datetime_of_transaction = DateTime.Today.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
            }) ;



            Ipd_patient_details.Add(new PatientDetail()
            {
                patient_name = "Manisha T Agalawe",
                year_of_birth = "1980",
                visit_id= "56666",//ipd id
                address = "pune -411038",
                patient_abha_id = "",
                patient_identification_proof = "Aadhaar Card",
                patient_identification_number = "502545687895",
                patient_mobile_number = "9650932602",
                transaction_type = "",
                uhid_number = "20420000587",
                admission_number = "1540069",
              //  department_admitted_name = "Gynac",

                department_visited_code = "00002",
                datetime_of_transaction = DateTime.Today.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
            });



            module_wise_kpi_IPD.patient_details = Ipd_patient_details;




            //  Root obj = new Root();
            //  obj.Output = output;

            output.metadata = metadata;


            result.module_wise_kpi.Add(module_wise_kpi_OPD);
            result.module_wise_kpi.Add(module_wise_kpi_IPD);


            /*  obj.Output.result.module_wise_kpi = new List<ModuleWiseKpi>();

              obj.Output.result.module_wise_kpi.Add(module_wise_kpi_OPD);

              obj.Output.result.module_wise_kpi.Add(module_wise_kpi_IPD);*/

            output.result.Add(result);


            return output;

        }


        //public Output Get_JSON_NMC(DateTime froDate, DateTime toDate, List<PatientDetail> opdPatientList, List<PatientDetail> ipdPatientList, IConfiguration configuration)
        //{



        //    Output output = new Output();
        //    output.result = new List<Result>();

        //    Metadata metadata = new Metadata() { code = 200, message = "transaction successful", timestamp = "02/04/2020 19:12:15" };

        //    //  List<Result> result = new List<Result>();

        //    Result result = new Result();

        //    result.module_wise_kpi = new List<ModuleWiseKpi>();



        //    result.from_date = froDate.ToString("dd/MM/yyyy HH:mm:ss");
        //    result.to_date = toDate.ToString("dd/MM/yyyy HH:mm:ss");
        //    result.hf_id_hmis = _configuration.GetValue<string>("HospitalSetting:HIMS_FacilityId"); ;
        //    result.hf_id_abdm = _configuration.GetValue<string>("HospitalSetting:ABDM_FacilityId");
        //    result.health_facility_name = _configuration.GetValue<string>("HospitalSetting:FacilityName");

        //    ModuleWiseKpi module_wise_kpi_OPD = new ModuleWiseKpi();


        //    //for opd patient
        //    module_wise_kpi_OPD.module_code = "01";
        //    module_wise_kpi_OPD.module_name = "OPD";
        //    module_wise_kpi_OPD.hmis_code = _configuration.GetValue<string>("HospitalSetting:hmis_code");
        //    module_wise_kpi_OPD.opd_count = opdPatientList.Count;

        //    // Initialize opd list
        //    module_wise_kpi_OPD.patient_details = new List<PatientDetail>();


        //    //Assign opd list
        //    module_wise_kpi_OPD.patient_details = opdPatientList;


        //    // for IPD patient
        //    ModuleWiseKpi module_wise_kpi_IPD = new ModuleWiseKpi();

        //    module_wise_kpi_IPD.module_code = "2";
        //    module_wise_kpi_IPD.module_name = "IPD";
        //    module_wise_kpi_IPD.hmis_code = _configuration.GetValue<string>("HospitalSetting:hmis_code");
        //    module_wise_kpi_IPD.ipd_count = ipdPatientList.Count();

        //    //Initialize ipd list
        //    module_wise_kpi_IPD.patient_details = new List<PatientDetail>();




        //    //Assign ipd list
        //    module_wise_kpi_IPD.patient_details = ipdPatientList;




        //    //  Root obj = new Root();
        //    //  obj.Output = output;



        //    result.module_wise_kpi.Add(module_wise_kpi_OPD);
        //    result.module_wise_kpi.Add(module_wise_kpi_IPD);



        //    // Assign Meta data
        //    output.metadata = metadata;

        //    // Add opd ,ipd result

        //    output.result.Add(result);



        //    // return JSON_NMC
        //    return output;
        //}

    }
}
